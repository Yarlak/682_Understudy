using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dubs_Agent : Agent {

	Rigidbody rBody;
    void Start () 
	{
		Time.timeScale = 0.25f;
        rBody = GetComponent<Rigidbody>();
    }

	public GameObject Target;
	public GameObject Target1;
	public bool is_bitch;
	public float action1;
	public float action2;
	
	public GameObject master;
	public GameObject other;
	
    public override void AgentReset()
    {
                
        // The agent fell
		
		
		this.transform.position = new Vector3(3.0f, 0.0f, 3.0f);
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;
		
		other.transform.position = new Vector3(3.0f, 0.0f, 2.0f);
		
        
        
		float tempOne = Random.value;
		float tempTwo = Random.value;
		
		int mult1 = 1;
		int mult2 = 1;
		
		if (tempOne > 0.5f)
		{
			mult1 = -1;
		}
		
		if (tempTwo > 0.5f)
		{
			mult2 = -1;
		}
		
		if (Target.GetComponent<dubs_reward>().is_active == 0 && Target1.GetComponent<dubs_reward>().is_active == 0)
		{
			// Move the target to a new spot
			Target.transform.position = new Vector3(Random.value * 3 + 1 , 0.5f, Random.value * mult1 * 3 + 1 );
			Target.GetComponent<dubs_reward>().is_active = 1;
			Target.GetComponent<Renderer>().material.color = Color.yellow;
			
			Target1.transform.position = new Vector3(Random.value * -3 - 1 , 0.5f, Random.value * mult2 * 3 -1);
			Target1.GetComponent<dubs_reward>().is_active = 1;
			Target1.GetComponent<Renderer>().material.color = Color.yellow;
		}
		
        
        
    }
	
	List<float> observation = new List<float>();
	public override void CollectObservations()
	{
		// Calculate relative position
		Vector3 relativePosition = Target.transform.position - this.transform.position;
		Vector3 relativePosition1 = Target1.transform.position - this.transform.position;
		Vector3 relativePosition2 = other.transform.position - this.transform.position;
		
		if (is_bitch)
		{
			AddVectorObs(master.GetComponent<dubs_Agent>().action1);
			AddVectorObs(master.GetComponent<dubs_Agent>().action2);
		}
		
		//other player relative position
		AddVectorObs(relativePosition2.x/5);
		AddVectorObs(relativePosition2.z/5);
		
		//TargetActive
		AddVectorObs(Target.GetComponent<dubs_reward>().is_active);
		AddVectorObs(Target1.GetComponent<dubs_reward>().is_active);
		
		
		// Relative position
		AddVectorObs(relativePosition.x/5);
		AddVectorObs(relativePosition.z/5);
		
		
		AddVectorObs(relativePosition1.x/5);
		AddVectorObs(relativePosition1.z/5);
		
		
		// Distance to edges of platform
		AddVectorObs((this.transform.position.x + 5)/5);
		AddVectorObs((this.transform.position.x - 5)/5);
		AddVectorObs((this.transform.position.z + 5)/5);
		AddVectorObs((this.transform.position.z - 5)/5);
		
		// Agent velocity
		AddVectorObs(rBody.velocity.x/5);
		AddVectorObs(rBody.velocity.z/5);
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
		if (Target.GetComponent<dubs_reward>().is_active == 1)
		{
			dist1 = distanceToTarget/10;
		}
		
		if (Target1.GetComponent<dubs_reward>().is_active == 1)
		{
			dist2 = distanceToTarget1/10;
		}
		
		if (dist2 < dist1)
		{
			AddReward(-1 * dist2);
		}else
		{
			AddReward(-1 * dist1);
		}
		

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
		
		if (is_bitch == false)
		{
			//action1 = vectorAction[2];
			//action2 = vectorAction[3];
			action1 = 0.0f;
			action2 = 0.0f;
			
		}
		
		
		
		//rBody.AddForce(controlSignal * speed);
		rBody.velocity = controlSignal * speed;
	 }
 
}
