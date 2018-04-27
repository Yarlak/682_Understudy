using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class single_Agent : Agent {

	Rigidbody rBody;
    void Start () 
	{
		//Time.timeScale = 0.25f;
        rBody = GetComponent<Rigidbody>();
    }

	public GameObject Target;
	public bool is_bitch;
	
    public override void AgentReset()
    {
       		
		//this.transform.position = new Vector3(3.0f, 0.0f, 3.0f);
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;
		

		
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
		
		if (Target.GetComponent<single_reward>().is_active == 0)
		{
			// Move the target to a new spot
			Target.transform.position = new Vector3(Random.value * mult2 * 3 + 1 * mult2 , 0.5f, Random.value * mult1 * 3 + 1 * mult1 );
			Target.GetComponent<single_reward>().is_active = 1;
			Target.GetComponent<Renderer>().material.color = Color.yellow;
			
		}
		
        
        
    }
	
	List<float> observation = new List<float>();
	public override void CollectObservations()
	{
		// Calculate relative position
		Vector3 relativePosition = Target.transform.position - this.transform.position;

		
		// Relative position
		AddVectorObs(relativePosition.x/5);
		AddVectorObs(relativePosition.z/5);
		
		
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
		
		bool yolo = false;
		// Reached target
		
		float dist1 = 100000000;
		
		
		// Getting closer
		if (Target.GetComponent<single_reward>().is_active == 1)
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
		
		previousDistance = distanceToTarget;


		// Actions, size = 2
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
		controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);		
		//rBody.AddForce(controlSignal * speed);
		rBody.velocity = controlSignal * speed;
	 }
 
}
