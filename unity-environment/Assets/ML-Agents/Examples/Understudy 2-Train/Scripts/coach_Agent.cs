using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coach_Agent : Agent {

	public float reset_time;
	public float reset_delay = 180.0f;
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
	
	public GameObject understudy;
	
	public Dictionary<string, float> team_commands = new Dictionary<string, float>();
	Rigidbody rBody;
    void Start () 
	{
		//Time.timeScale = 0.25f;
        rBody = GetComponent<Rigidbody>();
		team_commands.Add("p1_1", 0.0f);
		team_commands.Add("p1_2", 0.0f);
		team_commands.Add("p2_1", 0.0f);
		team_commands.Add("p2_2", 0.0f);
		
		
    }

	
	
    public override void AgentReset()
    {
                
        // The agent fell
		reset_time = Time.time + reset_delay;
		
		
			// Move the target to a new spot
		Target.transform.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
		Target.GetComponent<dubs3_reward>().is_active = 1;
		Target.GetComponent<Renderer>().material.color = Color.yellow;
			
		Target1.transform.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
		Target1.GetComponent<dubs3_reward>().is_active = 1;
		Target1.GetComponent<Renderer>().material.color = Color.yellow;
			
		other.GetComponent<dubs3_Agent>().Done();
		other1.GetComponent<dubs3_Agent>().Done();
		understudy.GetComponent<understudy1_agent>().Done();
        
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
			
		AddVectorObs(Target.GetComponent<dubs3_reward>().is_active);
		AddVectorObs(Target1.GetComponent<dubs3_reward>().is_active);
			
			
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
			Done();
		}
		

		// Actions, size = 2		
		
		team_commands["p1_1"] = vectorAction[0];
		team_commands["p1_2"] = vectorAction[1];
		team_commands["p2_1"] = vectorAction[2];
		team_commands["p2_2"] = vectorAction[3];
		
		
	 }
 
}
