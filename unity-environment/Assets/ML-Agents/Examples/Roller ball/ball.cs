using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour {


public GameObject player;
//public GameObject opponent;

Rigidbody r_body;

Vector3 hit_direction;

float up_mult = 5.0f;
float forward_mult = 10.0f;



	
	void OnCollisionEnter(Collision collision)
	{
		
		float p_reward = 0.0f;
		
		if (collision.collider.gameObject.tag == "score1" || collision.collider.gameObject.tag == "score2")
		{
			
			r_body.angularVelocity = Vector3.zero;
			r_body.velocity = Vector3.zero;
			if (collision.collider.gameObject.tag == "score1")
			{
				
				if (player.tag == "team1")
				{
					//p_reward = -20.0f;
				}else
				{
					p_reward = 100.0f;
				}
				
			}else if (collision.collider.gameObject.tag == "score2")
			{
				
				if (player.tag == "team1")
				{
					p_reward = 100.0f;
				}else
				{
					//p_reward = -20.0f;
				}
			}
			print(p_reward);
			print(p_reward.ToString());
			player.GetComponent<eAgent>().AddReward(p_reward);
			player.GetComponent<eAgent>().Done();
			//opponent.GetComponent<eAgent>().AddReward(-1.0f * p_reward);
			//opponent.GetComponent<eAgent>().Done();
		}
	}


	
	// Use this for initialization
	void Start () 
	{
		
		r_body = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
