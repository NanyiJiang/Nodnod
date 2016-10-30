using UnityEngine;
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

    public float portalForward = 6.0f;
    public float portalShift = 4.0f;
    public float beatSpeed = 2.0f;
    public float innerSphereRadius = 2.0f;
    public float outerSphereRadius = 3.0f;

    public float minimumBeatDT = 0.5f;
    float lastBeatTime = 0.0f;

    float bufferTime = 0.0f;
	void Start(){
        bufferTime = (portalForward - (innerSphereRadius + outerSphereRadius) / 2.0f) / beatSpeed;
        // Set position of the launchers

        Vector3 forwardV = new Vector3(0, 0, portalForward);
        Vector3 v1 = forwardV + new Vector3(0, -portalShift, 0);
        Vector3 v2 = forwardV + new Vector3(0, portalShift, 0);
        Vector3 v3 = forwardV + new Vector3(portalShift, 0, 0);
        Vector3 v4 = forwardV + new Vector3(-portalShift, 0, 0);

        v1 = v1.normalized * portalForward;
        v2 = v2.normalized * portalForward;
        v3 = v3.normalized * portalForward;
        v4 = v4.normalized * portalForward;

        leftSphere.transform.position = v1;
        rightSphere.transform.position = v2;
        topSphere.transform.position = v3;
        bottomSphere.transform.position = v4;

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
			else {
                var newLine = line.Replace('M', '0');
				numLinesInMeasure++;
				int[] action = new int[4];
				action[0] = int.Parse(newLine[0].ToString());
				action[1] = int.Parse(newLine[1].ToString());
				action[2] = int.Parse(newLine[2].ToString());
				action[3] = int.Parse(newLine[3].ToString());
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

        if (currTime < beats.Count) {
			while (beats [currTime].timestamp + bufferTime < timeElapsed) {
                currTime++;
				isUsed = false;
				if (currTime >= beats.Count)
					return;
			}
			if (!isUsed && timeElapsed - lastBeatTime > minimumBeatDT) {
				switch (beats [currTime].direction) {
				    case 0:
                        leftSphere.GetComponent<BeatGenerator>().SpawnBeat();
					    break;
				    case 1:
                        rightSphere.GetComponent<BeatGenerator>().SpawnBeat();
                        break;
				    case 2:
                        topSphere.GetComponent<BeatGenerator>().SpawnBeat();
                        break;
                    case 3:
                        bottomSphere.GetComponent<BeatGenerator>().SpawnBeat();
					    break;
				    default:
					    break;
                }
                lastBeatTime = timeElapsed;
            }
			isUsed = true;
		}
	}
}