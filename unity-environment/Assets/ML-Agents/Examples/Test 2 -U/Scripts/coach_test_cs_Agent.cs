using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class coach_test_cs_Agent : Agent {
	
	
	public float reset_time;
	public float reset_delay = 0.75f;
	public GameObject Target;
	public GameObject Target1;
	
	public float action1;
	public float action2;
	
	

	public GameObject other;
	public GameObject other1;
	
	public Vector3 relativePosition;
	public Vector3 relativePosition1;
	public Vector3 relativePosition2;
	public Vector3 relativePosition3;
	
	bool failed;
	float score;
	float start_time;
	int spawn_count = 0;
	Vector2[] spawn1 = new Vector2[100];
	Vector2[] spawn2 = new Vector2[100];
	
	StreamReader the_what;
	
	public Dictionary<string, float> team_commands = new Dictionary<string, float>();
	Rigidbody rBody;
    void Start () 
	{
		reset_delay = 2.0f;
		//Time.timeScale = 0.25f;
        rBody = GetComponent<Rigidbody>();
		team_commands.Add("p1_1", 0.0f);
		team_commands.Add("p1_2", 0.0f);
		team_commands.Add("p2_1", 0.0f);
		team_commands.Add("p2_2", 0.0f);
		
		the_what = new StreamReader("C:/Users/OH YEA/Documents/NN_Final Project/ML Agents/unity-environment/Assets/ML-Agents/Examples/test-cs/test_points.csv");
		int count = 0;
		while (!the_what.EndOfStream)
		{
			string the_line = the_what.ReadLine();
			string[] the_pos = the_line.Split(',');
			spawn1[count] = new Vector2(float.Parse(the_pos[0]), float.Parse(the_pos[1]));
			spawn2[count] = new Vector2(float.Parse(the_pos[2]), float.Parse(the_pos[3]));
			count += 1;
		}
		
		
		
    }

	
	
    public override void AgentReset()
    {
                
        // The agent fell
		if (spawn_count != 0)
		{
			float temp_score = reset_delay - (Time.time - start_time);
			score += temp_score * 100;
		}
		
		
		start_time = Time.time;
		reset_time = Time.time + reset_delay;
		spawn_count += 1;
		
		if (spawn_count != 100 && failed == false)
		{
			// Move the target to a new spot
			Target.transform.position = new Vector3(spawn1[spawn_count].x, 0.5f, spawn1[spawn_count].y);
			Target.GetComponent<dubs3_test_cs_reward>().is_active = 1;
			Target.GetComponent<Renderer>().material.color = Color.yellow;
				
			Target1.transform.position = new Vector3(spawn2[spawn_count].x, 0.5f, spawn2[spawn_count].y);
			Target1.GetComponent<dubs3_test_cs_reward>().is_active = 1;
			Target1.GetComponent<Renderer>().material.color = Color.yellow;
				
			other.GetComponent<dubs3_test_cs_Agent>().Done();
			other1.GetComponent<dubs3_test_cs_Agent>().Done();			
			
		}else
		{
			print("Game Complete - Score: " + score.ToString());
		}
        
    }
	
	List<float> observation = new List<float>();
	public override void CollectObservations()
	{
		// Calculate relative position
		relativePosition = Target.transform.position - this.transform.position;
		relativePosition1 = Target1.transform.position - this.transform.position;
		relativePosition2 = other.transform.position - this.transform.position;
		relativePosition3 = other1.transform.position - this.transform.position;
		
		
		AddVectorObs(relativePosition2.x/5);
		AddVectorObs(relativePosition2.z/5);
		
		AddVectorObs(relativePosition3.x/5);
		AddVectorObs(relativePosition3.z/5);
			
		AddVectorObs(Target.GetComponent<dubs3_test_cs_reward>().is_active);
		AddVectorObs(Target1.GetComponent<dubs3_test_cs_reward>().is_active);
			
			
			// Relative position
		AddVectorObs(relativePosition.x/5);
		AddVectorObs(relativePosition.z/5);
			
			
		AddVectorObs(relativePosition1.x/5);
		AddVectorObs(relativePosition1.z/5);
		
		
		
	
	}
	
	public float speed = 10;
	private float previousDistance = float.MaxValue;
	private float previousDistance1 = float.MaxValue;

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		// Rewards
		
		// Time penalty
		AddReward(-1.0f);

		// Fell off platform
		if (Time.time > reset_time)
		{
			failed = true;
			Done();
		}
		

		// Actions, size = 2		
		
		team_commands["p1_1"] = vectorAction[0];
		team_commands["p1_2"] = vectorAction[1];
		team_commands["p2_1"] = vectorAction[2];
		team_commands["p2_2"] = vectorAction[3];
		
		
	 }
 
}
