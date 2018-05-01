using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class generatepoints : MonoBehaviour {
	
	int counter;
	int max_count = 100;
	Vector2 point1;
	Vector2 point2;
	
	StringBuilder the_what;
	// Use this for initialization
	void Start () 
	{
		the_what = new StringBuilder();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (counter < max_count)
		{
			
			
			point1 = new Vector2(Random.value * 8 - 4, Random.value * 8 - 4);
			point2 = new Vector2(Random.value * 8 - 4, Random.value * 8 - 4);
			
			string point1_s = point1.x.ToString() + "," + point1.y.ToString();
			string point2_s = point2.x.ToString() + "," + point2.y.ToString();
			//p1_x, p1_z, p2_x, p2_z
			the_what.AppendLine(point1_s + "," + point2_s);
			
			counter += 1;
		}
		print(point1);
		if (counter == max_count)
		{
			string the_path = "C:/Users/OH YEA/Documents/NN_Final Project/ML Agents/unity-environment/Assets/ML-Agents/Examples/test-coach/test_points.csv";
			File.AppendAllText(the_path, the_what.ToString());
			counter += 1;
		}
		
	}	
}
