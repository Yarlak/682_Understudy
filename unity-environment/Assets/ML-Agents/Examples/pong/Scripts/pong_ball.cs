using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pong_ball : MonoBehaviour {

public GameObject team1;
public GameObject team2;
Rigidbody r_body;

public Vector3 curDirection;

public float speed = 13.0f;
public float hitDelay = 0.01f;

	void OnCollisionEnter(Collision collision)
	{
		
		float p_reward = 0.0f;
		
		Vector3 newDirection = Vector3.zero;
		
		string hitTag = collision.gameObject.tag;
		
		
		if (Time.time > hitDelay)
		{
			if (hitTag == "score1" || hitTag == "score2")
			{
				float restart_direction = 0.0f;
				if (hitTag == "score1")
				{
					float temp_reward = team1.GetComponent<pongAgent>().distanceToTarget;
					
					team1.GetComponent<pongAgent>().AddReward(-5.0f * temp_reward - 20.0f);
					team2.GetComponent<pongAgent>().AddReward(100.0f);
					restart_direction = 1.0f;
				}else
				{
					float temp_reward = team2.GetComponent<pongAgent>().distanceToTarget;
					print(temp_reward);
					team1.GetComponent<pongAgent>().AddReward(100.0f);
					team2.GetComponent<pongAgent>().AddReward(-5.0f * temp_reward - 20.0f);
					restart_direction = -1.0f;
				}
				
				team1.GetComponent<pongAgent>().Done();
				team2.GetComponent<pongAgent>().Done();
				transform.position = new Vector3(0.0f, 1.0f, 0.0f);
				newDirection = new Vector3 (0.0f, 0.0f, restart_direction * speed);
				
				
			}else if (hitTag == "bounds")
			{
				newDirection = new Vector3 (-1.0f * curDirection.x, 0.0f, curDirection.z * 1.5f);
				
			}else if (hitTag == "team1" || hitTag == "team2")
			{
				GameObject temp = collision.gameObject;
				float x_add = temp.GetComponent<Rigidbody>().velocity.x;
				newDirection = new Vector3 (curDirection.x + x_add/10.0f, 0.0f, -1.5f * curDirection.z);
				
				
			}
				
			newDirection = newDirection/newDirection.magnitude;
			r_body.velocity = newDirection * speed;
			curDirection = newDirection;
			hitDelay = Time.time + 0.03f;
			
		}
		
		
		
		
			
		
	}

	
	// Use this for initialization
	void Start () 
	{
		r_body = gameObject.GetComponent<Rigidbody>();
		
		transform.position = new Vector3(0.0f, 1.0f, 0.0f);
		curDirection = new Vector3 (0.0f, 0.0f, -1.0f);
		curDirection = curDirection/curDirection.magnitude;
		r_body.velocity = curDirection * speed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
