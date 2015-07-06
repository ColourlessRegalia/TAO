using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class AttackManager : MonoBehaviour {
	
	private Animation anim;
	public int elementNo; //element states: 1 fire, 2 ice, 3 wind, 4 earth
	public float buttonTimer; //timer that runs
	public float buttonTimerMax; //timer value that buttonTimer resets to, for easy changing of values
	public bool nextMove; //check if player can perform next move
//	public bool elementPause; //prevents player from switching elements at end of combo
	public bool enemyUpdateDelay;
	private bool stateChanged; //check if attack state was just changed, prevent state from being locked
	public GameObject FireSP;
    public GameObject IceSP;
	public Material fireAtk; //material for elements
	public Material iceAtk;
	public Material windAtk;
	public Material earthAtk;
	

	// Use this for initialization
	void Start () {
		buttonTimerMax = 1.5f;
		buttonTimer = buttonTimerMax;
		nextMove = false;
	//	elementPause = false;
		enemyUpdateDelay = false;
		elementNo = 1;
		anim = GetComponent<Animation> ();
		var cube = gameObject.transform.FindChild ("Attack Cube");
		cube.GetComponent<MeshRenderer> ().material = fireAtk;

		var player = GetComponent<ThirdPersonLogic>();
		player.attackState = 0;
	}

	//changing attack state
	void changeState () {
		var cube = gameObject.transform.FindChild ("Attack Cube");
		if (stateChanged) {
			if (cube.GetComponent<MeshRenderer>().material.name == "FireAtkCubeMat (Instance)") {
			//	player.attackState = 0;
				stateChanged = false;
			}
			if (cube.GetComponent<MeshRenderer>().material.name == "IceAtkCubeMat (Instance)") {
			//	player.attackState = 0;//4;
				stateChanged = false;
			}
		}
	}

	// Update is called once per frame
	void Update () {
	//	print ("AttackManager ElementNo + " + elementNo);
		var player = GetComponent<ThirdPersonLogic>();
		var cube = gameObject.transform.FindChild ("Attack Cube");
		//if (!anim.IsPlaying("SanzusIdle") && !anim.IsPlaying("SanzusRunning")) { //when animation is not playing attack stuff
		//if (anim.isPlaying) { //element only changes AFTER animation is played/attack is done
		if (anim.IsPlaying("SanzusIdle") || anim.IsPlaying("SanzusRunning") || nextMove) {
			if (elementNo == 1) {
				cube.GetComponent<MeshRenderer> ().material = fireAtk;
				changeState ();
			}
			if (elementNo == 2) {
				cube.GetComponent<MeshRenderer> ().material = iceAtk;
				changeState ();
			}
			if (elementNo == 3) {
				cube.GetComponent<MeshRenderer> ().material = windAtk;
				changeState ();
			}
			if (elementNo == 4) {
				cube.GetComponent<MeshRenderer> ().material = earthAtk;
				changeState ();
			}
		}

		if (buttonTimer > 0 && player.isAttacking && player.finishMove) { //timer for button pressing
				buttonTimer -= Time.deltaTime;
				nextMove = true;
		} if (buttonTimer <= 0 && player.isAttacking && nextMove) {
			if (player.attackState != 3 || player.attackState != 7) { //if player is not at the last combo string 
				StartCoroutine(drawBack(0.05f));
			}
		}

		//print (elementNo);
		if (Application.loadedLevelName != "Base") { //disables element switching when not out in the battles
			if (CrossPlatformInputManager.GetButtonDown ("Element +")/* && elementPause == false*/) { //cycles elements 1 to 4, back to 1
				if (elementNo <= 4) {
					elementNo++;
					stateChanged = true;
				}
				if (elementNo > 4) {
					elementNo = 1;
					stateChanged = true;
				}
			}

			if (CrossPlatformInputManager.GetButtonDown ("Element -")/* && elementPause == false*/) {
				if (elementNo >= 1) { //cycles elements 4 to 1, back to 4
					elementNo--;
					stateChanged = true;
				}
				if (elementNo < 1) {
					elementNo = 4;
					stateChanged = true;
				}
			}
		}

		if (CrossPlatformInputManager.GetButtonDown ("Fire")) {
			if (PlayerData.playerMana == 100) 
            {
                if (elementNo == 1)
                {
                    Instantiate(FireSP, transform.position, FireSP.transform.rotation);
                    AreaDamageEnemies(transform.position, 6, 10);
                    enemyUpdateDelay = true;
                    PlayerData.playerMana = 0;
                    StartCoroutine(enemyUpdate(2.0f));
                }
                if (elementNo == 2)
                {
                    Instantiate(IceSP, transform.position, IceSP.transform.rotation);
                }
			}
		}

		if (CrossPlatformInputManager.GetButtonDown ("Slash")) {
			//print ("AttackManager: " +player.attackState);
			//print(cube.GetComponent<MeshRenderer>().material);
			if (Application.loadedLevelName != "Base") {
				if (nextMove) { //while player can perform the next move
					if (cube.GetComponent<MeshRenderer>().material.name == "FireAtkCubeMat (Instance)") {
						if (player.attackState == 0) {
//							buttonTimer = buttonTimerMax;
//							nextMove = false;
//						player.onAttack = false;
//							player.attackState = 1;
							return;
						}

						if (player.attackState == 1) {
//							buttonTimer = buttonTimerMax;
//							nextMove = false;
//							player.onAttack = false;
//							player.attackState = 2;
							return;
						}

						if (player.attackState == 2) {
//							buttonTimer = buttonTimerMax;
//							nextMove = false;
//							player.onAttack = false;
//							player.attackState = 3;
//							StartCoroutine (drawBack (2.0f)); //runs a timer to reset the combo string after end of combo
							return;
						}
					} //close fire attack combo

					if (cube.GetComponent<MeshRenderer>().material.name == "IceAtkCubeMat (Instance)") {
						if (player.attackState == 0) {
//							buttonTimer = buttonTimerMax;
//							nextMove = false;
//							player.onAttack = false;
//							player.attackState = 1;
							return;
						}
						
						if (player.attackState == 1) {
//							buttonTimer = buttonTimerMax;
//							nextMove = false;
//							player.onAttack = false;
//							player.attackState = 2;
							return;
						}
						
						if (player.attackState == 2) {
//							buttonTimer = buttonTimerMax;
//							nextMove = false;
//							player.onAttack = false;
//							player.attackState = 3;
//							StartCoroutine (drawBack (2.0f)); //runs a timer to reset the combo string after end of combo
							return;
						}
					} //close ice attack combo

				}
			} //close non-battlefield attack disabler
		}
	}

	//AOE damage effect
	void AreaDamageEnemies(Vector3 location, float radius, int damage) { 
		Collider[] objectsInRange = Physics.OverlapSphere(location, radius); 
		foreach (Collider col in objectsInRange) { 
			var enemy = col.GetComponent<EnemyLogic>(); 
			if (enemy != null) { 
				enemy.health -= damage;
			}
		}
	}
	IEnumerator enemyUpdate (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		enemyUpdateDelay = false;
	}

	IEnumerator drawBack (float waitTime) {
	//	print (waitTime);
		yield return new WaitForSeconds (waitTime);
//		var player = GetComponent<ThirdPersonLogic>();
		var cube = gameObject.transform.FindChild ("Attack Cube");
//		if (cube.GetComponent<MeshRenderer>().material.name == "FireAtkCubeMat (Instance)") {
//			if (player.attackState < 4) {
//				anim.Play("SanzusReturnIdle");
//				print ("Spot on!");
//			}
//		}

		if (cube.GetComponent<MeshRenderer>().material.name == "IceAtkCubeMat (Instance)") {
		//	anim.CrossFade("SanzusIdle");
		}
	//	StartCoroutine (resetAttack (0.1f));
	}

//	IEnumerator resetAttack (float waitTime) {
//		yield return new WaitForSeconds (waitTime);
//		var player = GetComponent<ThirdPersonLogic>();
//		var cube = gameObject.transform.FindChild ("Attack Cube");
//		if (cube.GetComponent<MeshRenderer>().material.name == "FireAtkCubeMat (Instance)") {
//			player.attackState = 0;
//		}
//		if (cube.GetComponent<MeshRenderer>().material.name == "IceAtkCubeMat (Instance)") {
//			player.attackState = 4;
//		}
//		cube.gameObject.SetActive(false);
//		buttonTimer = buttonTimerMax;
//		player.onAttack = false;
//		player.finishMove = false;
//	//	elementPause = false;
//		nextMove = false;
//		player.isAttacking = false;
//	}
}