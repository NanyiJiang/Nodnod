﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Beat {
	public int direction;
	public int step;
	public float timestamp;
}

public class ReadTextFile : MonoBehaviour {
	public GameObject leftSphere;
	public GameObject rightSphere;
	public GameObject topSphere;
	public GameObject bottomSphere;

	public TextAsset textFile;     // drop your file here in inspector
	public List<Beat> beats = new List<Beat>();
	public float bpm;
	public float offset;
	public int currTime = 0;
	public float timeElapsed = 0;
	public int numMeasures = 0;
	public int numLines1 = 0;
	public int numLinesInMeasure = 0;
	public bool isUsed = false;
	public List<int> measureEnds = new List<int>();
	public List<int> measureStarts = new List<int>();
	public List<int> numLinesInMeasures = new List<int>();
	public List<int[]> actions = new List<int[]> ();
	void Start(){

		string text = textFile.text;  //this is the content as string
		//byte[] byteText = textFile.bytes;  //this is the content as byte array

		string[] lines = text.Split("\n" [0]);
		foreach (string line in lines)
		{
			if (line.Contains ("#OFFSET:")) {
				offset = float.Parse((line.Split (':')) [1]);
			}
			else if (line.Contains ("#BPMS:")) {
				bpm = float.Parse((line.Split (':')) [1]);
			} 
			else if (line.Contains(",") || line.Contains(";")) {
				numMeasures++;
				measureStarts.Add (numLines1);
				numLines1 += numLinesInMeasure;
				measureEnds.Add (numLines1);
				numLinesInMeasures.Add (numLinesInMeasure);
				numLinesInMeasure = 0;
			} 
			else if (line.Length == 5) {
				numLinesInMeasure++;
				int[] action = new int[4];
				action[0] = int.Parse(line[0].ToString());
				action[1] = int.Parse(line[1].ToString());
				action[2] = int.Parse(line[2].ToString());
				action[3] = int.Parse(line[3].ToString());
				actions.Add(action);
			}
		} 
		int currMeasure = 0;
		int i = 0;
		while (i < numLines1) {
			if (i < (int)measureEnds [currMeasure]) {
				int[] action = actions [i];
				int stepValue = 1;
				int maxIndex = System.Array.IndexOf (action, 1);
				if (maxIndex == -1) {
					maxIndex = System.Array.IndexOf (action, 2);
					stepValue = 2;
				}
				if (maxIndex == -1) {
					maxIndex = System.Array.IndexOf (action, 3);
					stepValue = 3;
				}

				if (maxIndex != -1) {
					Beat beat = new Beat();
					float multiplier = (float)currMeasure + ((float)i - (float)(measureStarts [currMeasure])) / ((float)(numLinesInMeasures [currMeasure]));

					beat.timestamp = (float)((4 * 60 / bpm) * offset + ((4 * 60 / bpm) * multiplier));
					beat.direction = maxIndex;
					beat.step = stepValue;
					beats.Add (beat);
				}
				i++;
			} else {
				currMeasure++;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;

		var material1 = leftSphere.GetComponent<Renderer> ().material;
		material1.color = Color.white;
		var material2 = rightSphere.GetComponent<Renderer> ().material;
		material2.color = Color.white;
		var material3 = topSphere.GetComponent<Renderer> ().material;
		material3.color = Color.white;
		var material4 = bottomSphere.GetComponent<Renderer> ().material;
		material4.color = Color.white;

		if (currTime < beats.Count) {
			while (beats [currTime].timestamp < timeElapsed) {
				currTime++;
				isUsed = false;
				if (currTime >= beats.Count)
					return;
			}
			if (!isUsed) {
				switch (beats [currTime].direction) {
				case 0:
					material1.color = Color.magenta;
					break;
				case 1:
					material2.color = Color.blue;
					break;
				case 2:
					material3.color = Color.red;
					break;
				case 3:
					material4.color = Color.cyan;
					break;
				default:
					break;
				}
			}
			isUsed = true;
		}
	}
}