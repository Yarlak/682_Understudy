using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class single_t_reward : MonoBehaviour {

	public int is_active;
	GameObject player;
	
	// Use this for initialization
	void OnTriggerEnter(Collider collision)
	{
		
		player = collision.gameObject;
		
		if (collision.gameObject.tag == "player" && is_active == 1)
		{
			is_active = 0;
			gameObject.GetComponent<Renderer>().material.color = Color.red;
			player.GetComponent<single_t_Agent>().AddReward(300.0f);
			
			player.GetComponent<single_t_Agent>().Done();
			
			
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
