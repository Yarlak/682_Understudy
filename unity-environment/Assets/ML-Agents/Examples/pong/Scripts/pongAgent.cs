using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pongAgent : Agent {

	Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;
	public GameObject theBall;
	public GameObject opponent;
	public float distanceToTarget;
    public override void AgentReset()
    {
        transform.position = new Vector3 (0.0f, 1.0f, transform.position.z);
    }
	
	List<float> observation = new List<float>();
	public override void CollectObservations()
	{
		// Calculate relative position
		Vector3 relativePosition = Target.position - this.transform.position;
		
		// Relative position
		AddVectorObs(relativePosition.x/8.3f);
		AddVectorObs(relativePosition.z/21.0f);
		
		// Distance to edges of platform
		AddVectorObs((this.transform.position.x + 8.3f)/8.3f);
		AddVectorObs((this.transform.position.x - 8.3f)/8.3f);
		AddVectorObs((opponent.transform.position.x + 8.3f)/8.3f);
		AddVectorObs((opponent.transform.position.x - 8.3f)/8.3f);
		
		// Agent velocity
		AddVectorObs(rBody.velocity.x/8.3f);
		
		Vector3 tempBall = theBall.GetComponent<Rigidbody>().velocity;
		
		AddVectorObs(tempBall.x/8.3f);
		AddVectorObs(tempBall.z/21.0f);
		
	}
	
	public float speed = 3;
	private float previousDistance = float.MaxValue;

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		// Rewards
		distanceToTarget = Vector3.Distance(this.transform.position, 
												  Target.position);
		
		// Reached target
		
		
		// Getting closer
		

		// Time penalty
		AddReward(-0.05f);

		// Fell off platform
		
		previousDistance = distanceToTarget;

		// Actions, size = 2
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
		
		if (controlSignal.x == 0)
		{
			rBody.velocity = Vector3.zero;
		}else if (controlSignal.x == 1)
		{
			rBody.velocity = new Vector3 (6.0f, 0.0f, 0.0f);
		}else
		{
			rBody.velocity = new Vector3 (-6.0f, 0.0f, 0.0f);
		}
		
		//rBody.AddForce(controlSignal * speed);
	 }
 
}
