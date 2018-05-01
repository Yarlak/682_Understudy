using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class getpoints : MonoBehaviour {

	StreamReader the_what;
	// Use this for initialization
	Vector2[] point1 = new Vector2[100];
	Vector2[] point2 = new Vector2[100];
	void Start () 
	{
		the_what = new StreamReader("C:/Users/OH YEA/Documents/NN_Final Project/ML Agents/unity-environment/Assets/ML-Agents/Examples/test-coach/test_points.csv");
		int count = 0;
		while (!the_what.EndOfStream)
		{
			string the_line = the_what.ReadLine();
			string[] the_pos = the_line.Split(',');
			point1[count] = new Vector2(float.Parse(the_pos[0]), float.Parse(the_pos[1]));
			point2[count] = new Vector2(float.Parse(the_pos[2]), float.Parse(the_pos[3]));
			count += 1;
		}
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
