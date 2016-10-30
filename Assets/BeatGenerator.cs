using UnityEngine;
using System.Collections;

public class BeatGenerator : MonoBehaviour {
	public GameObject Beat;
	public float TimeElapsed = 0.0f;
	public float BeatTime = 1.0f;
	public float speed = 2.0f;
	public Transform Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnBeat() {
		GameObject beat = GameObject.Instantiate (Beat);
		beat.transform.position = transform.position;
		beat.transform.LookAt (Target.transform);
		beat.GetComponent<Rigidbody> ().velocity = -(transform.position - Target.position).normalized * speed;
		beat.GetComponent<BeatBall>().SetHightlight (4, Target.transform.position);
	}
}
