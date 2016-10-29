using UnityEngine;
using System.Collections;

public class Beat : MonoBehaviour {
	public GameObject ExplosionRegular;
	public GameObject ExplosionTimeout;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MarkForDestroy() {
//		GameObject explosion = GameObject.Instantiate (ExplosionRegular);
//		explosion.transform.position = transform.position;
//		Destroy (explosion, 1.0f);
//		Destroy (gameObject);
	}

	public void Timeout() {
		GameObject explosion = GameObject.Instantiate (ExplosionTimeout);
		explosion.transform.position = transform.position;
		Destroy (explosion, 1.0f);
		Destroy (gameObject);
	}

	public void SetHightlight(float distance, Vector3 worldPoint) {
//		var renderer = GetComponent<MeshRenderer> ();
//		renderer.material.SetFloat ("Distance", distance);
//		float[] wp = {worldPoint.x, worldPoint.y, worldPoint.z};
//		renderer.material.SetFloatArray ("World Point", wp);
	}
}
