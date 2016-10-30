using UnityEngine;
using System.Collections;

public class BeatBall : MonoBehaviour {
	public GameObject ExplosionRegular;
	public GameObject ExplosionTimeout;
	public GameObject Crosshair;
	public float TargetTime = 10.0f;
	public float LockonTime = 0.0f;
	public bool markedForDestroy = false;
	bool targeted = false;

	// Use this for initialization
	void Start () {
		Crosshair.active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (targeted) {
			targeted = false;
		} else {
			LockonTime = Mathf.Max (LockonTime - Time.deltaTime * 5, 0);
		}
	}

	public void MarkForDestroy() {
//		GameObject explosion = GameObject.Instantiate (ExplosionRegular);
//		explosion.transform.position = transform.position;
//		Destroy (explosion, 1.0f);
//		Destroy (gameObject);
		markedForDestroy = true;
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

	public void SetTargeted(float deltaTime) {
		targeted = true;
		if (markedForDestroy) {
			Crosshair.active = true;
			LockonTime += deltaTime;
			float progress = LockonTime / TargetTime;
			Crosshair.transform.localScale = Mathf.Lerp (5f, 0.8f, progress) * Vector3.one;
			var angle = Crosshair.transform.eulerAngles;
			angle.z = Mathf.Lerp (50.0f, 0.8f, progress);
			Color color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp (0.0f, 1.0f, progress));
			var material = Crosshair.GetComponent<Renderer> ().material;
			material.color = color;

			Crosshair.transform.eulerAngles = angle;
			if (LockonTime > TargetTime) {
				GameObject explosion = GameObject.Instantiate (ExplosionRegular);
				explosion.transform.position = transform.position;
				Destroy (explosion, 1.0f);
				Destroy (gameObject);

                GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                manager.Hit();
            }
		}
	}
}
