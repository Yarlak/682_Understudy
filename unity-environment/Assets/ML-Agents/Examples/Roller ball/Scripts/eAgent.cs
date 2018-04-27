using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eAgent : Agent {

Rigidbody rBody;
Rigidbody b_rBody;

public float speed = 20.0f;
public float maxSpeed = 10;
private float previousDistance = float.MaxValue;
private float prev_dist_ball_to_goal = float.MaxValue;
public GameObject ball;
public Vector3 hit_direction;

float up_mult = 5.0f;
float forward_mult = 20.0f;

float stopTime;

Vector3 ballRespawn;
Vector3 agentRepawn;

//public GameObject opponent;

public Transform opponent_goal;


	void OnCollisionEnter(Collision collision)
	{
		
		if (collision.collider.gameObject.tag == "ball")
		{
			//AddReward(10.0f);
			hit_direction = collision.collider.gameObject.transform.position - transform.position;
			hit_direction = hit_direction/hit_direction.magnitude;
			
			
			collision.collider.gameObject.GetComponent<Rigidbody>().velocity = new Vector3 (hit_direction.x * forward_mult, up_mult, hit_direction.z * forward_mult);
		}else if (collision.collider.gameObject.tag == "wall")
		{
			AddReward(-1.0f);
			print("wall");
		}
	}



    void Start () 
	{
        rBody = GetComponent<Rigidbody>();
		b_rBody = ball.GetComponent<Rigidbody>();
		transform.position = new Vector3 (1.6f, 1.6f, 1.5f);
		speed = 0.25f;
		
    }


    public override void AgentReset()
    {
        
		
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
		
		if (gameObject.tag == "team1")
		{
			stopTime = Time.time + 120.0f;
			b_rBody.velocity = Vector3.zero;
			b_rBody.angularVelocity = Vector3.zero;
		
			ballRespawn = Random.insideUnitSphere;
			ball.transform.position = new Vector3 (ballRespawn.x * 15.0f, ballRespawn.y * ballRespawn.y, ballRespawn.z * 10.0f);
			//opponent.GetComponent<eAgent>().AgentReset();
		}
		
		
		
		agentRepawn = Random.insideUnitSphere;
		transform.position = new Vector3 (agentRepawn.x * 15.0f, 1.6f, agentRepawn.z * 10.0f);
        
    }
	
	public override void CollectObservations()
	{
		Vector3 relativePosition = ball.transform.position - this.transform.position;

		
		AddVectorObs((this.transform.position.x + 25)/25);
		AddVectorObs((this.transform.position.x - 25)/25);
		AddVectorObs((this.transform.position.z + 15)/15);
		AddVectorObs((this.transform.position.z - 15)/15);
		
		
		AddVectorObs(relativePosition.x/25);
		AddVectorObs(relativePosition.z/15);
		
		AddVectorObs(rBody.velocity.x/25);
		AddVectorObs(rBody.velocity.z/15);
		
		
		//AddVectorObs(b_rBody.velocity.x/25);
		//AddVectorObs(b_rBody.velocity.y);
		//AddVectorObs(b_rBody.velocity.z/15);
		
		relativePosition = opponent_goal.position - this.transform.position;
		
		AddVectorObs(relativePosition.x/25);
		AddVectorObs(relativePosition.z/15);
	}
	
	public override void AgentAction(float[] vectorAction, string textAction)
	{
		 float distanceToBall = Vector3.Distance(this.transform.position, ball.transform.position);
		 float dist_ball_to_goal = Vector3.Distance(ball.transform.position, opponent_goal.position);
   
		
		// Getting closer
		
		if (distanceToBall < previousDistance)
		{
			//AddReward(0.1f);
		}
		
		AddReward(-0.05f);

		// Time penalty

		// Fell off platform
		
		previousDistance = distanceToBall;
		prev_dist_ball_to_goal = dist_ball_to_goal;

		// Actions, size = 2
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
		controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
		
		rBody.AddForce(controlSignal * speed, ForceMode.VelocityChange);
		//transform.Translate(controlSignal * 0.2f);
		
		
		
		
		if (rBody.velocity.magnitude > 0.0f)
		{
			//rBody.velocity = Vector3.zero;
			//rBody.angularVelocity = Vector3.zero;
		}
		if (Time.time > stopTime && gameObject.tag == "team1")
		{
			Done();
		}
		
	}
}
