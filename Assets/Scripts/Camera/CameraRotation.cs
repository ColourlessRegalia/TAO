using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class CameraRotation : MonoBehaviour {

	private float speed;
	private bool isLeft;
	private bool rotate;
	private bool hoizontalLock;
	private bool verticalLock;
	private bool snapBack;

	// Use this for initialization
	void Start () {
		speed = 3.0f;
		snapBack = true;
		var player = GameObject.Find ("Sanzus");
		transform.position = player.transform.position;
	}

	void ClampRotation(float minAngle, float maxAngle, float clampAroundAngle = 0) {
		//clampAroundAngle is the angle you want the clamp to originate from
		//For example a value of 90, with a min=-45 and max=45, will let the angle go 45 degrees away from 90
		
		//Adjust to make 0 be right side up
		clampAroundAngle += 180;
		
		//Get the angle of the z axis and rotate it up side down
		float x = transform.rotation.eulerAngles.x - clampAroundAngle;
		
		x = WrapAngle(x);
		
		//Move range to [-180, 180]
		x -= 180;
		
		//Clamp to desired range
		x = Mathf.Clamp(x, minAngle, maxAngle);
		
		//Move range back to [0, 360]
		x += 180;
		
		//Set the angle back to the transform and rotate it back to right side up
		transform.rotation = Quaternion.Euler(x + clampAroundAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	}
	
	//Make sure angle is within 0,360 range
	float WrapAngle(float angle) {
		//If its negative rotate until its positive
		while (angle < 0)
			angle += 360;
		
		//If its to positive rotate until within range
		return Mathf.Repeat(angle, 360);
	}

	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown ("Camera")) {
			snapBack = true;
		}

		float dL = CrossPlatformInputManager.GetAxis ("Dodge");
		if (dL != 0) {
			transform.eulerAngles = new Vector3 (0,0,0);
		}

		var player = GameObject.Find ("Sanzus");
		transform.position = player.transform.position;
		if (snapBack) { //to reset rotation for camera to be behind player
			transform.rotation = player.transform.rotation;
			snapBack = false;
		}

		if (rotate) {
			if (isLeft) {
				transform.position += transform.right * speed * Time.deltaTime;
			} else {
				transform.position -= transform.right * speed * Time.deltaTime;
			}
		}
		var fT = CrossPlatformInputManager.GetAxis("Camera Horizontal");
		var iT = CrossPlatformInputManager.GetAxis ("Camera Vertical");
		//print (fT);
		if (fT > 0) {
			rotate = true;
			isLeft = false;
			verticalLock = true;
		} else if (fT < 0) {
			rotate = true;
			isLeft = true;
			verticalLock = true;
		} else if (fT == 0) {
			rotate = false;
			verticalLock = false;
		}

		if (iT < 0) {
			rotate = true;
			isLeft = false;
			hoizontalLock = true;
		} else if (iT > 0) {
			rotate = true;
			isLeft = true;
			hoizontalLock = true;
		} else if (iT == 0) {
			rotate = false;
			hoizontalLock = false;
		}

		//print ("isLeft: " + isLeft + ", rotate: " + rotate);
		if (fT != 0 && hoizontalLock == false) {
			transform.RotateAround (player.transform.position, Vector3.up, CrossPlatformInputManager.GetAxis ("Camera Horizontal") * speed);
		}

		if (iT != 0 && verticalLock == false) {
			transform.RotateAround (player.transform.position, transform.right, CrossPlatformInputManager.GetAxis ("Camera Vertical") * speed);
			ClampRotation(-40, 50, 0);
		}
	}
}
