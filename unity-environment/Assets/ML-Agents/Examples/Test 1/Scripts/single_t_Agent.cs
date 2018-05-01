using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class single_t_Agent : Agent {

	Rigidbody rBody;
	public GameObject tracker;
	bool failed;
	float score;
	float start_time;
	int spawn_count = 0;
	Vector2[] spawn1 = new Vector2[100];
	
	StreamReader the_what;
	float reset_time;
	float reset_delay = 180.0f;
	
    void Start () 
	{
		Time.timeScale = 0.25f;
        rBody = GetComponent<Rigidbody>();

		the_what = new StreamReader("C:/Users/OH YEA/Documents/NN_Final Project/ML Agents/unity-environment/Assets/ML-Agents/Examples/Test2-L2/test_points.csv");
		int count = 0;
		while (!the_what.EndOfStream)
		{
			string the_line = the_what.ReadLine();
			string[] the_pos = the_line.Split(',');
			spawn1[count] = new Vector2(float.Parse(the_pos[0]), float.Parse(the_pos[1]));
			count += 1;
		}
		
		
		
    }

	public GameObject Target;
	public Vector3 relativePosition;
	public float action1;
	public float action2;
	
    public override void AgentReset()
    {
       		
		//this.transform.position = new Vector3(3.0f, 0.0f, 3.0f);
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;
		
		if (spawn_count != 0)
		{
			float temp_score = reset_delay - (Time.time - start_time);
			score += temp_score;
		}
		
		
		start_time = Time.time;
		reset_time = Time.time + reset_delay;
		spawn_count += 1;
		
		if (spawn_count != 100 && failed == false)
		{
			// Move the target to a new spot
			Target.transform.position = new Vector3(spawn1[spawn_count].x, 0.5f, spawn1[spawn_count].y);
			Target.GetComponent<single_t_reward>().is_active = 1;
			Target.GetComponent<Renderer>().material.color = Color.yellow;			
			
		}else
		{
			print("Game Complete - Score: " + score.ToString());
		}
		
        
        
    }
	

	public override void CollectObservations()
	{
		// Calculate relative position
		relativePosition = Target.transform.position - this.transform.position;
		
		// Relative position
		AddVectorObs(relativePosition.x/5);
		AddVectorObs(relativePosition.z/5);
		
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
		//Instantiate(tracker, transform.position, transform.rotation);
		float distanceToTarget = Vector3.Distance(this.transform.position, Target.transform.position);
		
		bool yolo = false;
		// Reached target
		
		float dist1 = 100000000;
		
		
		// Getting closer
		if (Target.GetComponent<single_t_reward>().is_active == 1)
		{
			dist1 = distanceToTarget/10;
		}
		
		AddReward(-1 * dist1);
		
		// Time penalty
		AddReward(-1.0f);

		// Fell off platform
		if (this.transform.position.y < -1.0)
		{
			Done();
			AddReward(-1.0f);
		}
		
		if (Time.time > reset_time)
		{
			failed = true;
			Done();
		}
		
		previousDistance = distanceToTarget;

		// Actions, size = 2
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
		controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
		action1 = vectorAction[0];
		action2 = vectorAction[1];
		//rBody.AddForce(controlSignal * speed);
		rBody.velocity = controlSignal * speed;
	 }
 
}
