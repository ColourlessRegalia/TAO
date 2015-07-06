using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private Animation anim;
	private int speed;
	private int intSpeed;
	private float jumpSpeed;
	private float dodgeSpeed;
	private bool canEvade;
	private bool canMove;
	private Transform cameraTransform;
	private Vector3 walkDirection;
	private Vector3 gravity;

	// Use this for initialization
	void Start () {
		canEvade = true;
		canMove = true;
		gravity = Vector3.zero;
		intSpeed = 4;
		speed = intSpeed;
		jumpSpeed = 5.0f;
		dodgeSpeed = 3.0f;
		cameraTransform = Camera.main.transform;
		anim = GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 forward = cameraTransform.TransformDirection (Vector3.forward);
		forward.y = 0f; 
		forward = forward.normalized; 
		Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

		if (canMove) {
			float h = CrossPlatformInputManager.GetAxis ("Horizontal");
			float v = CrossPlatformInputManager.GetAxis ("Vertical");
			walkDirection = (h * right + v * forward);

			if (h != 0 || v!= 0) {
				if (!GetComponent<ThirdPersonLogic>().isAttacking) {
					anim.CrossFade("SanzusRunning");
				}
			} else if (h == 0 && v == 0 && !GetComponent<ThirdPersonLogic>().isAttacking) {
					anim.Play("SanzusIdle");
			}
		}

		//print (walkDirection);
		if (walkDirection.sqrMagnitude > 1f) {
			walkDirection = walkDirection.normalized;
		}
		if(walkDirection.magnitude > 0.05f)
		{
			transform.LookAt(transform.position + walkDirection);
		}
		//transform.position += walkDirection * speed * Time.deltaTime;

		var attack = GetComponent<ThirdPersonLogic>();
		if (attack.isAttacking == true) {
			speed = 0;
		} else {
			speed = intSpeed;
		}

		walkDirection *= speed;
		if (GetComponent<CharacterController> ().isGrounded == false) {
			gravity += Physics.gravity * Time.deltaTime;
		} else {
			gravity = Vector3.zero;
			if (Application.loadedLevelName != "Base") {
				float dL = CrossPlatformInputManager.GetAxis ("Dodge");
				if (dL == 1 && canEvade) {
					GetComponent<Animation> ().Play ("DodgeLeft");
					canEvade = false;
					StartCoroutine (resetEvade (2.0f));
				}
				if (dL == -1 && canEvade) {
					GetComponent<Animation> ().Play ("DodgeRight");
					canEvade = false;
					StartCoroutine (resetEvade (2.0f));
				}

				if (GetComponent<Animation>().IsPlaying("DodgeLeft")){
					canMove = false;
					gravity.x = -dodgeSpeed;
				} else if (GetComponent<Animation>().IsPlaying("DodgeRight")){
					canMove = false;
					gravity.x += dodgeSpeed;
				} else {
					canMove = true;
				}
			}

			if (CrossPlatformInputManager.GetButtonDown ("Jump") && Application.loadedLevelName != "Base") {
				gravity.y = jumpSpeed;
			}
		}

		walkDirection += gravity;
		GetComponent<CharacterController> ().Move (walkDirection * Time.deltaTime);
	}

	IEnumerator resetEvade (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		canEvade = true;
	}
}