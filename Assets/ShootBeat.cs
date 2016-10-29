using UnityEngine;
using System.Collections;

public class ShootBeat : MonoBehaviour {
	public GameObject crosshair;
	public GameObject sight;
	public LayerMask layerMask;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		var crosshairPosition = crosshair.transform.position;
		RaycastHit rayHit;
		Ray ray = new Ray();
//		ray.origin = transform.position + UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye);
//		Debug.Log ("crosshair:" + ray.origin + "	:" + ray.origin);
//		ray.direction = (crosshairPosition - ray.origin).normalized;
//		ray.direction = crosshair.transform.forward;
		ray.direction = sight.transform.position - crosshair.transform.position;
		ray.origin = crosshair.transform.position - 2 * ray.direction;
		Debug.DrawRay (ray.origin, ray.direction, Color.green);
		if (!Physics.Raycast(ray, out rayHit, 100, layerMask))
			return;
		Debug.Log ("shot");
		rayHit.collider.gameObject.GetComponent<BeatBall> ().SetTargeted (Time.deltaTime);
	}
}
