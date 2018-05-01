using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class dubs3_test_cs_Agent : Agent {

	Rigidbody rBody;
	public string p_name;
	
	public float reset_delay;
	public float reset_time;
	
	float go_time;
	
	bool failed;
	float score;
	float start_time;
	int spawn_count = 0;
	Vector2[] spawn1 = new Vector2[100];
	Vector2[] spawn2 = new Vector2[100];
	
	StreamReader the_what;
	
	
    void Start () 
	{
		Time.timeScale = 100.0f;
        rBody = gameObject.GetComponent<Rigidbody>();
		p_name = gameObject.name;
		reset_delay = 180.0f;
		
		
		if (!is_player)
		{
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
		
		
    }

	public GameObject Target;
	public GameObject Target1;
	public bool is_player;
	public float action1;
	public float action2;
	
	public GameObject master;
	public GameObject other;
	
    public override void AgentReset()
    {
                
        // The agent fell
		
		
		//this.transform.position = new Vector3(3.0f, 0.0f, 3.0f);
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;
		
		//other.transform.position = new Vector3(3.0f, 0.0f, 2.0f); 
		
		if (!is_player)
		{
			if (spawn_count != 0 && failed == false)
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
				Target.GetComponent<dubs3_test_cs_reward>().is_active = 1;
				Target.GetComponent<Renderer>().material.color = Color.yellow;
					
				Target1.transform.position = new Vector3(spawn2[spawn_count].x, 0.5f, spawn2[spawn_count].y);
				Target1.GetComponent<dubs3_test_cs_reward>().is_active = 1;
				Target1.GetComponent<Renderer>().material.color = Color.yellow;
					
				other.GetComponent<dubs3_test_cs_Agent>().Done();		
				
			}else
			{
				print("Game Complete - Score: " + score.ToString());
			}
		}
		
		
        
    }
	
	List<float> observation = new List<float>();
	public override void CollectObservations()
	{
		// Calculate relative position
		Vector3 relativePosition = Target.transform.position - this.transform.position;
		Vector3 relativePosition1 = Target1.transform.position - this.transform.position;
		Vector3 relativePosition2 = other.transform.position - this.transform.position;
		
		if (is_player)
		{
			AddVectorObs(master.GetComponent<dubs3_test_cs_Agent>().action1);
			AddVectorObs(master.GetComponent<dubs3_test_cs_Agent>().action2);
		}else
		{
			AddVectorObs(relativePosition2.x/5);
			AddVectorObs(relativePosition2.z/5);
			
			AddVectorObs(Target.GetComponent<dubs3_test_cs_reward>().is_active);
			AddVectorObs(Target1.GetComponent<dubs3_test_cs_reward>().is_active);
			
			
			// Relative position
			AddVectorObs(relativePosition.x/5);
			AddVectorObs(relativePosition.z/5);
			
			
			AddVectorObs(relativePosition1.x/5);
			AddVectorObs(relativePosition1.z/5);
		}
		
		
		//TargetActive
		
		
		
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
		float distanceToTarget = Vector3.Distance(this.transform.position, Target.transform.position);
		float distanceToTarget1 = Vector3.Distance(this.transform.position, Target1.transform.position);
		bool yolo = false;
		bool yolo1 = false;
		// Reached target
		
		float dist1 = 100000000;
		float dist2 = 100000000;
		
		
		// Getting closer
		if (Target.GetComponent<dubs3_test_cs_reward>().is_active == 1)
		{
			dist1 = distanceToTarget/10;
		}
		
		if (Target1.GetComponent<dubs3_test_cs_reward>().is_active == 1)
		{
			dist2 = distanceToTarget1/10;
		}
		
		float dist_reward;
		if (dist2 < dist1)
		{
			dist_reward = -1 * dist2;
		}else
		{
			dist_reward = -1 * dist1;
		}
		
		if (is_player)
		{
			master.GetComponent<dubs3_test_cs_Agent>().AddReward(dist_reward);
		}else
		{
			AddReward(dist_reward);
		}
		
		if (Time.time > reset_time)
		{
			failed = true;
			Done();
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
		
		if (is_player == false)
		{
			//action1 = vectorAction[2];
			//action2 = vectorAction[3];
			action1 = vectorAction[2];
			action2 = vectorAction[3];
			
		}		
		
		
		//rBody.AddForce(controlSignal * speed);
		rBody.velocity = controlSignal * speed;
	 }
 
}
