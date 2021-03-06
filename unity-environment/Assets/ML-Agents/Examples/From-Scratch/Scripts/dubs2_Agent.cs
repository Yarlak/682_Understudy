﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dubs2_Agent : Agent {

	Rigidbody rBody;
	
	public GameObject Target;
	public GameObject Target1;
	public bool is_player;
	public float action1;
	public float action2;
	
	public GameObject master;
	public GameObject other;

	public float reset_time;
	public float reset_delay = 180.0f;
	
    void Start () 
	{
		//Time.timeScale = 0.25f;
        rBody = GetComponent<Rigidbody>();
    }

	
	
    public override void AgentReset()
    {
                
        // The agent fell
		reset_time = Time.time + reset_delay;
		
		//this.transform.position = new Vector3(3.0f, 0.0f, 3.0f);
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;
		
		//other.transform.position = new Vector3(3.0f, 0.0f, 2.0f);
		
		
		// Move the target to a new spot
		Target.transform.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4 );
		Target.GetComponent<dubs2_reward>().is_active = 1;
		Target.GetComponent<Renderer>().material.color = Color.yellow;
			
		Target1.transform.position = new Vector3(Random.value * 8 - 4 , 0.5f, Random.value * 8 - 4);
		Target1.GetComponent<dubs2_reward>().is_active = 1;
		Target1.GetComponent<Renderer>().material.color = Color.yellow;
		
        
        
    }
	
	List<float> observation = new List<float>();
	public override void CollectObservations()
	{
		// Calculate relative position
		Vector3 relativePosition = Target.transform.position - this.transform.position;
		Vector3 relativePosition1 = Target1.transform.position - this.transform.position;
		Vector3 relativePosition2 = other.transform.position - this.transform.position;
		
		if (is_player)
		{
			AddVectorObs(master.GetComponent<dubs2_Agent>().action1);
			AddVectorObs(master.GetComponent<dubs2_Agent>().action2);
		}
		
		//other player relative position
		if (!is_player)
		{
			AddVectorObs(relativePosition2.x/5);
			AddVectorObs(relativePosition2.z/5);
			
			//TargetActive
			AddVectorObs(Target.GetComponent<dubs2_reward>().is_active);
			AddVectorObs(Target1.GetComponent<dubs2_reward>().is_active);
			
			
			// Relative position
			AddVectorObs(relativePosition.x/5);
			AddVectorObs(relativePosition.z/5);
			
			
			AddVectorObs(relativePosition1.x/5);
			AddVectorObs(relativePosition1.z/5);
			
		}
		
		
		// Distance to edges of platform
		AddVectorObs((this.transform.position.x + 5)/5);
		AddVectorObs((this.transform.position.x - 5)/5);
		AddVectorObs((this.transform.position.z + 5)/5);
		AddVectorObs((this.transform.position.z - 5)/5);
		
		// Agent velocity
		//AddVectorObs(rBody.velocity.x/5);
		//AddVectorObs(rBody.velocity.z/5);
	}
	
	public float speed = 10;
	private float previousDistance = float.MaxValue;
	private float previousDistance1 = float.MaxValue;

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		// Rewards
		float distanceToTarget = Vector3.Distance(this.transform.position, Target.transform.position);
		float distanceToTarget1 = Vector3.Distance(this.transform.position, Target1.transform.position);
		bool yolo = false;
		bool yolo1 = false;
		// Reached target
		
		float dist1 = 100000000;
		float dist2 = 100000000;
		
		
		// Getting closer
		if (Target.GetComponent<dubs2_reward>().is_active == 1)
		{
			dist1 = distanceToTarget/10;
		}
		
		if (Target1.GetComponent<dubs2_reward>().is_active == 1)
		{
			dist2 = distanceToTarget1/10;
		}
		
		float dist_reward;
		if (dist2 < dist1)
		{
			dist_reward = -1 * dist2;
		}else
		{
			dist_reward = -1 * dist1;
		}
		
		if (is_player)
		{
			other.GetComponent<dubs2_Agent>().AddReward(dist_reward);
		}
		
		AddReward(dist_reward);
		

		// Time penalty
		AddReward(-1.0f);

		// Fell off platform
		if (this.transform.position.y < -1.0)
		{
			Done();
			AddReward(-1.0f);
		}
		previousDistance = distanceToTarget;
		previousDistance1 = distanceToTarget1;

		// Actions, size = 2
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
		controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
		
		if (is_player == false)
		{
			action1 = vectorAction[2];
			action2 = vectorAction[3];
			
		}
		
		if (Time.time > reset_time)
		{
			Done();
		}
		
		
		
		//rBody.AddForce(controlSignal * speed);
		rBody.velocity = controlSignal * speed;
	 }
 
}
