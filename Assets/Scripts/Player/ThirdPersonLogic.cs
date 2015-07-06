using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class ThirdPersonLogic : MonoBehaviour {

	//private Transform aim;
	//private float spawnDistance;
	private float recoverTimer; 
	private float recoverTimerSet;
	private Vector3 startPosition; //Player starting position in the map.
	private Vector3 startRotation; //Player starting rotation in the map.
	public bool isAttacking; // bool to check whether the player is attacking.
	public bool onAttack; 
	public bool isHit;  // bool to check whether the player is being hit.
	public bool finishMove; 
	public int attackState;
	public int enemyNo; //number of enemies after player
	public GameObject bullet;
	private int elementNo;

	enum ElementState {fire, ice, earth, wind};
	ElementState elementState;
	ParticleSystem hurtParticle;

	public List<GameObject> enemyRefList;
	// Use this for initialization

	void Start () {
		//spawnDistance = 1.0f;
		attackState = 0;
		enemyNo = 0;
		recoverTimerSet = 0.5f;
		recoverTimer = recoverTimerSet;
		startPosition = PlayerData.spawnPosition;
		startRotation = PlayerData.spawnRotation;
		transform.position = startPosition;
		transform.eulerAngles = startRotation;
		isAttacking = false;
		onAttack = false;
		isHit = false;
		finishMove = false;
		//aim = gameObject.transform.FindChild ("Aim");
		elementState = ElementState.fire;
		hurtParticle = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
//		print ("ThirdPersonLogic ElementNo + " + elementNo);
//		print ("AttackState + " + attackState);
		elementNo = GetComponent<AttackManager> ().elementNo;
		//attack stuff 
		if (Application.loadedLevelName != "Base") {
			/*if (CrossPlatformInputManager.GetButtonDown ("Fire")) {
				Instantiate (bullet, aim.transform.position + spawnDistance * transform.forward, transform.rotation);
				isAttacking = true;
			}

			if (CrossPlatformInputManager.GetButtonUp ("Fire")) {
				StartCoroutine (canMove (0.3f));
			}*/

			if (CrossPlatformInputManager.GetButtonDown ("Slash")) {
				//print ("slash");
				attackState ++;
				var cube = gameObject.transform.FindChild ("Attack Cube");
				if (!isAttacking) {
					cube.gameObject.SetActive (true);
					isAttacking = true;
					finishMove = true;
					if (cube.GetComponent<MeshRenderer>().material.name == "FireAtkCubeMat (Instance)") {
						attackState = 1;
					}

					if (cube.GetComponent<MeshRenderer>().material.name == "IceAtkCubeMat (Instance)") {
						attackState = 1;
					}
				}

				if (cube.gameObject.activeInHierarchy) {
					if (isAttacking) {
						if (elementNo == 1){
							if (attackState == 1) {
							if (/*!onAttack &&*/ finishMove) {
								//GetComponent<Animation> ().Play ("Attack1");
								GetComponent<Animation> ().Play ("FireAttack01");
								onAttack = true;
						//		finishMove = false;
//								GetComponent<AttackManager>().elementPause = false;
								StopCoroutine("FinishAttack");
								StartCoroutine("FinishAttack",(GetComponent<Animation>()["FireAttack01"].clip.length + 0.23f));
							}
						}
						if (attackState == 2) {
							if (/*!onAttack &&*/ finishMove) {
								//GetComponent<Animation> ().Play ("Attack2");
								GetComponent<Animation> ().Play ("FireAttack02");
								onAttack = true;
						//		finishMove = false;
							//	GetComponent<AttackManager>().elementPause = false;
								StopCoroutine("FinishAttack");
								StartCoroutine("FinishAttack",(GetComponent<Animation>()["FireAttack02"].clip.length + 0.23f));
							}
						}
						if (attackState == 3) {
						//	if (!onAttack && finishMove) {
								//GetComponent<Animation> ().Play ("Attack3");
								GetComponent<Animation> ().Play ("FireAttack03");
								onAttack = true;
						//		finishMove = false;
							//	GetComponent<AttackManager>().elementPause = true;
								StopCoroutine("FinishAttack");
								StartCoroutine("FinishAttack",(GetComponent<Animation>()["FireAttack03"].clip.length + 0.23f));
						//	}
							}
						}
						if (elementNo == 2){
							if (attackState == 1) {
						//	if (!onAttack && finishMove) {
								GetComponent<Animation> ().Play ("Attack4Ice");
								onAttack = true;
						//		finishMove = false;
							//	GetComponent<AttackManager>().elementPause = false;
								StopCoroutine("FinishAttack");
								StartCoroutine("FinishAttack",(GetComponent<Animation>()["Attack4Ice"].clip.length));
						//	}
						}
						if (attackState == 2) {
						//	if (!onAttack && finishMove) {
								GetComponent<Animation> ().Play ("Attack5Ice");
								onAttack = true;
						//		finishMove = false;
							//	GetComponent<AttackManager>().elementPause = false;
								StopCoroutine("FinishAttack");
								StartCoroutine("FinishAttack",(GetComponent<Animation>()["Attack5Ice"].clip.length));
						//	}
						}
						if (attackState == 3) {
						//	if (!onAttack && finishMove) {
								GetComponent<Animation> ().Play ("Attack6Ice");
								onAttack = true;
						//		finishMove = false;
							//	GetComponent<AttackManager>().elementPause = true;
								StopCoroutine("FinishAttack");
								StartCoroutine("FinishAttack",(GetComponent<Animation>()["Attack6Ice"].clip.length));
						//	}
							}
						}
					}
				} // close if cube is active loop
			}
		}
	}

	void AttackAnimation(){
		switch (elementState) 
		{
			case ElementState.fire:
				if (!onAttack && finishMove)
				{
					//GetComponent<Animation>().Play("FireAttack01");
				}
				break;

			case ElementState.ice:
			{
				if ((!onAttack && finishMove))
				{
					//GetComponent<Animation>().Play("Attack4Ice");
				}
			}
			break;
		}
	}

	IEnumerator canMove (float waitTime) {
		yield return new WaitForSeconds (waitTime);
	}
	
	IEnumerator FinishAttack (float waitTime) {

		yield return new WaitForSeconds (waitTime);
		attackState = 0;
//		finishMove = true;
		isAttacking = false;
		onAttack = false;
//		GetComponent<Animation> ().Play("SanzusReturnIdle");
	}

    //void OnTriggetEnter(Collider collider)
    //{
    //    print("Hello");
    //    if (collider.gameObject.tag == "Enemy")
    //    {
    //        print("Here is where the hit animation should play");
    //        hurtParticle.Play();
    //    }
            //if (recoverTimer > 0)
            //{
            //    recoverTimer -= Time.deltaTime;
            //}
            //if (recoverTimer <= 0)
            //{
            //    isHit = false;
            //    recoverTimer = recoverTimerSet;
            //}
    //}

	void OnTriggerEnter (Collider collider) {
		if (Application.loadedLevelName == "Outskirt") {
			if (collider.transform.tag == "Exit1") {
				PlayerData.spawnPosition = new Vector3 (1.40f, 0f, 34.34f);
				PlayerData.spawnRotation = new Vector3 (0f, 166f, 0f);
				Application.LoadLevel("Volcano");
				print ("teleport");
				enemyRefList.Clear();
			}

			if (collider.transform.tag == "Exit2") {
				PlayerData.spawnPosition = new Vector3 (0f, 0f, 0f);
				PlayerData.spawnRotation = new Vector3 (0f, 0f, 0f);
				Application.LoadLevel("Base");
				enemyRefList.Clear();
			}
		}

		if (Application.loadedLevelName == "Volcano") {
			if (collider.transform.tag == "Exit1") {
				PlayerData.spawnPosition = new Vector3 (5.63f, 0f, 18.16f);
				PlayerData.spawnRotation = new Vector3 (0f, 222.14f, 0f);
				Application.LoadLevel("Outskirt");
				enemyRefList.Clear();
			}
		}
	}
}