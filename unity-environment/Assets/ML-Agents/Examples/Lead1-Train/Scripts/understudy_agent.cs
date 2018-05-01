using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class understudy_agent : Agent {

	
	public GameObject tutor;
	
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
		
		Vector3 relativePosition = tutor.GetComponent<single_Agent>().relativePosition;
				
		// Relative position
		AddVectorObs(relativePosition.x/5);
		AddVectorObs(relativePosition.z/5);
		
		// Distance to edges of platform
		AddVectorObs((tutor.transform.position.x + 5)/5);
		AddVectorObs((tutor.transform.position.x - 5)/5);
		AddVectorObs((tutor.transform.position.z + 5)/5);
		AddVectorObs((tutor.transform.position.z - 5)/5);
		
	}
	


	public override void AgentAction(float[] vectorAction, string textAction)
	{
		
		float action1 = tutor.GetComponent<single_Agent>().action1;
		float action2 = tutor.GetComponent<single_Agent>().action2;
		
		float temp_reward1 = Mathf.Abs(action1 - vectorAction[0]);
		float temp_reward2 = Mathf.Abs(action2 - vectorAction[1]);
		
		temp_reward1 = temp_reward1 * temp_reward1;
		temp_reward2 = temp_reward2 * temp_reward2;
		
		AddReward(-1.0f * temp_reward1);
		AddReward(-1.0f * temp_reward2);
		
		
	 }
 
}
