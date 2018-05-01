using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class understudy1_agent : Agent {

	
	public GameObject tutor;
	public GameObject tutor2;
	public GameObject Target;
	public GameObject Target1;
	
	public GameObject player;
	
    void Start () 
	{
		//Time.timeScale = 0.25f;
    }

    public override void AgentReset()
    {
		
    }
	
	List<float> observation = new List<float>();
	public override void CollectObservations()
	{
		// Calculate relative position
		
		Vector3 relativePosition = Target.transform.position - tutor.transform.position;
		Vector3 relativePosition1 = Target1.transform.position - tutor.transform.position;
		
		Vector3 relativePosition3 = player.transform.position - tutor.transform.position;
				
		// Relative position
		AddVectorObs(relativePosition3.x/5);
		AddVectorObs(relativePosition3.z/5);
			
		AddVectorObs(Target.GetComponent<dubs3_reward>().is_active);
		AddVectorObs(Target1.GetComponent<dubs3_reward>().is_active);
			
			
		// Relative position
		AddVectorObs(relativePosition.x/5);
		AddVectorObs(relativePosition.z/5);
			
			
		AddVectorObs(relativePosition1.x/5);
		AddVectorObs(relativePosition1.z/5);
		
		// Distance to edges of platform
		AddVectorObs((tutor.transform.position.x + 5)/5);
		AddVectorObs((tutor.transform.position.x - 5)/5);
		AddVectorObs((tutor.transform.position.z + 5)/5);
		AddVectorObs((tutor.transform.position.z - 5)/5);
		
	}
	


	public override void AgentAction(float[] vectorAction, string textAction)
	{
		
		float action1 = tutor.GetComponent<dubs3_Agent>().action1;
		float action2 = tutor.GetComponent<dubs3_Agent>().action2;
		
		
		float temp_reward1 = Mathf.Abs(action1 - vectorAction[0]);
		float temp_reward2 = Mathf.Abs(action2 - vectorAction[1]);
		
		temp_reward1 = temp_reward1 * temp_reward1;
		temp_reward2 = temp_reward2 * temp_reward2;
		
		float action3 = tutor2.GetComponent<coach_Agent>().team_commands["p2_1"];
		float action4 = tutor2.GetComponent<coach_Agent>().team_commands["p2_2"];
		
		float temp_reward3 = Mathf.Abs(action3 - vectorAction[2]);
		float temp_reward4 = Mathf.Abs(action4 - vectorAction[3]);
		
		temp_reward3 = temp_reward3 * temp_reward3;
		temp_reward4 = temp_reward4 * temp_reward4;
		
		AddReward(-1.0f * temp_reward1);
		AddReward(-1.0f * temp_reward2);
		AddReward(-1.0f * temp_reward3);
		AddReward(-1.0f * temp_reward4);
		
	 }
 
}
