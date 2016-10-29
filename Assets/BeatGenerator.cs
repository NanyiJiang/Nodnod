﻿using UnityEngine;
using System.Collections;

public class BeatGenerator : MonoBehaviour {
	public GameObject Beat;
	public float TimeElapsed = 0.0f;
	public float BeatTime = 1.0f;
	public Transform Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		TimeElapsed += Time.deltaTime;
		if (TimeElapsed > BeatTime) {
			SpawnBeat ();
			TimeElapsed -= BeatTime;
		}
	}

	void SpawnBeat() {
		GameObject beat = GameObject.Instantiate (Beat);
		beat.transform.position = transform.position;
		beat.GetComponent<Rigidbody> ().velocity = -(transform.position - Target.position).normalized * 0.5f;
		beat.GetComponent<Beat>().SetHightlight (4, Target.transform.position);
	}
}
