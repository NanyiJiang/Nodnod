using UnityEngine;
using System.Collections;

public class ShootBeat : MonoBehaviour {
	public GameObject crosshair;
	public LayerMask layerMask;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		var crosshairPosition = crosshair.transform.position;
		RaycastHit rayHit;
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(crosshairPosition), out rayHit, 100, layerMask))
			return;
		rayHit.collider.gameObject.GetComponent<BeatBall> ().SetTargeted (Time.deltaTime);
	}
}
