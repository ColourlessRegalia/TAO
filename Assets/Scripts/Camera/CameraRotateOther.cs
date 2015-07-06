using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CameraRotateOther : MonoBehaviour {
	
	// Target following
	private Vector3 cameraRotation;

	void Start () {
		cameraRotation = new Vector3 (20, 0, 0);
		transform.eulerAngles = cameraRotation;
	}
}