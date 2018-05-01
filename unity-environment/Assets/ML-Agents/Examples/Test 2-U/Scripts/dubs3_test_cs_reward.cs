using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dubs3_test_cs_reward : MonoBehaviour {

	public GameObject coach_test_cs;
	public GameObject other_reward;
	public int is_active;
	GameObject player;
	public GameObject player1;
	public GameObject player2;
	// Use this for initialization
	void OnTriggerEnter(Collider collision)
	{
		
		player = collision.gameObject;
		
		if (collision.gameObject.tag == "player" && is_active == 1)
		{
			is_active = 0;
			gameObject.GetComponent<Renderer>().material.color = Color.red;
			player1.GetComponent<dubs3_test_cs_Agent>().AddReward(300.0f);
			player2.GetComponent<dubs3_test_cs_Agent>().AddReward(300.0f);
			
			if (other_reward.GetComponent<dubs3_test_cs_reward>().is_active == 0)
			{
				coach_test_cs.GetComponent<dubs3_test_cs_Agent>().Done();
			}
			
		}
		
	}
	
	
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
