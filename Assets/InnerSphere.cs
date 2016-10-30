using UnityEngine;
using System.Collections;

public class InnerSphere : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Beat") {
			other.gameObject.GetComponent<BeatBall> ().Timeout ();

            GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            manager.Miss();
		}
	}
}
