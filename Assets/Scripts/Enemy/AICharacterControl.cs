using System.Collections;
using UnityEngine;

[RequireComponent(typeof (NavMeshAgent))]
public class AICharacterControl : MonoBehaviour
{
	public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
    public Transform target; // target to aim for
	public GameObject LHand;
	public GameObject RHand;
	private Vector3 steeringDirection;
	private Vector3 targetPosition;
	private float speed;
	private float detectAreaMax; //max detect distance
	private float detectAreaMin; //min detect distance
	private float attackIdleArea; //area where enemies wait if not in the front attack row
	private float detectAreaFlee; //distance where it flees
	private float RNGtimer;
	private float RNGtimerSet;
	private float waitingTimer;
	private float waitingTimerSet;
	private float wanderTimer;
	private float wanderTimerSet;
	private float attackIdleTimer;
	private float attackIdleTimerSet;
	private float circleOffset;
	private float circleRadius;
	private int atkRNG;
	private int listPos;
	private bool enableWandering;
	private bool move;
	private bool runRNG;
	public bool isWandering;
	public bool flee;
	public bool attack;
	public bool attackExt;
	public bool hitBoundary;
	public bool returnToSpawn;
	public string enemyID;

    // Use this for initialization
	private void Start()
    {
		target = GameObject.Find ("Sanzus").transform;
		// get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<NavMeshAgent>();

		agent.updateRotation = false;
	    agent.updatePosition = true;
		speed = 5.0f;
		detectAreaMax = 100f;
		detectAreaMin = 3f;
		detectAreaFlee = 120f;
		attackIdleArea = 20f;
		RNGtimerSet = 1.0f;
		RNGtimer = RNGtimerSet;
		waitingTimerSet = 2.0f;
		waitingTimer = waitingTimerSet; 
		attackIdleTimerSet = UnityEngine.Random.Range (1.0f, 3.0f);
		attackIdleTimer = attackIdleTimerSet;
		attack = false;
		attackExt = false;
		flee = false;
		runRNG = true;
		enableWandering = true;
		isWandering = false;
		hitBoundary = false;
		returnToSpawn = false;
		agent.SetDestination (target.position);
	}

	// Update is called once per frame
    private void Update() {
		if (move) {
			//print ("move");
			transform.LookAt(target);
			agent.SetDestination (target.position);
		} else {
			if (enableWandering) {
				if (!isWandering) {
					wander();
				} else {
					if (wanderTimer > 0) {
						wanderTimer -= Time.deltaTime;
					} else if (wanderTimer <= 0) {
						wanderTimer = wanderTimerSet;
						isWandering = false;
					}
				}
			} else {
				agent.SetDestination(transform.position+=Vector3.zero);
			}
		}
			
		var player = GameObject.Find ("Sanzus").GetComponent<ThirdPersonLogic>();
		var spawn = GameObject.Find ("Enemy Spawn").GetComponent<EnemySpawner> ();
        if (target != null) {
			if ((target.position - transform.position).sqrMagnitude > detectAreaMax) {
				move = false; //if player is beyond enemy's range, don't chase
				enableWandering = true;
			} else {
				if (player.enemyNo < 3) {
					if (!attack & !flee & player.enemyRefList.Count < 1) {
						attack = true; 
						move = true;
						enableWandering = false;
						player.enemyNo++;
						//print ("added enemy, " +player.enemyNo);
					}
				} else {
					if (!attack & !attackExt) {
						attackExt = true;
						move = true;
						enableWandering = false;
						player.enemyRefList.Add(this.gameObject);
						PlayerData.enemyList++;
						listPos = PlayerData.enemyList;
						gameObject.name = "Enemy" +listPos;
						enemyID = gameObject.name;
					}
				}
			}
		} else {
			move = false;
			enableWandering = true;
        }

		if (spawn.enemyCount <= 1) {
			LHand.GetComponent<EnemyAttackManager>().canHit = false;
			RHand.GetComponent<EnemyAttackManager>().canHit = false;
			if (attack) {
				attack = false;
			}
			if (attackExt) {
				attackExt = false;
			}
			flee = true;
			if ((target.position - transform.position).sqrMagnitude < detectAreaFlee) {
				transform.LookAt(target);
				transform.rotation = Quaternion.Inverse(target.rotation);
				Vector3 moveDir = transform.position - player.transform.position;
				transform.position += moveDir.normalized * speed * Time.deltaTime;
			} else {
				move = false;
				flee = false;
				enableWandering = true;
			}
		}

		if (attack) { 
			if ((target.position - transform.position).sqrMagnitude > detectAreaMax) {
				player.enemyNo--;
				if (player.enemyNo < 0) {
					player.enemyNo = 0;
				}
				//print ("minus enemy, " +player.enemyNo);
				attack = false;
				move = false;
				enableWandering = true;
				LHand.GetComponent<EnemyAttackManager>().canHit = false;
				RHand.GetComponent<EnemyAttackManager>().canHit = false;
			} else if ((target.position - transform.position).sqrMagnitude < detectAreaMin){
				move = false;
				enableWandering = false;
				if (RNGtimer > 0) {
					if (player.isHit == false) {
						RNGtimer -= Time.deltaTime;
						if (attackIdleTimer > 0) {
							attackIdleTimer -= Time.deltaTime;
						}
						if (attackIdleTimer <= 0) {
							attackIdleShift();
						}
					}
				} if (RNGtimer <= 0) {
					runAttackRNG();
				}
			} else {
				if (player.isHit == true) {
					move = false;
					enableWandering = false;
				}
				move = true;
				enableWandering = false;
			}
		}

		if (attackExt) {
			if ((target.position - transform.position).sqrMagnitude > detectAreaMax && player.enemyRefList.Count > 0) {
				for (int i =0; i < player.enemyRefList.Count; i++) {
					if (player.enemyRefList[i].gameObject.name == enemyID) {
						move = false;
						enableWandering = true;
						LHand.GetComponent<EnemyAttackManager>().canHit = false;
						RHand.GetComponent<EnemyAttackManager>().canHit = false;
						player.enemyRefList.RemoveAt(i);
						attackExt = false;
					}
				}
			} else if ((target.position - transform.position).sqrMagnitude < attackIdleArea){
				move = false;
				enableWandering = false;
				if (waitingTimer > 0) {
					waitingTimer -= Time.deltaTime;
				} else if (waitingTimer <= 0) {
					checkEnemyNo();
				}
			} else {
				move = true;
				enableWandering = false;
			}
		}

		if (hitBoundary) {
			wanderTimer = wanderTimerSet;
			isWandering = false;
			turnAround();
		}

		if (returnToSpawn) {
			if (!attack && !attackExt && !flee) {
				isWandering = false;
				returnToArea ();
			}
		} 
	}

	void attackIdleShift() {
		var direction = UnityEngine.Random.Range (1, 3);
		if (direction == 1) {
			targetPosition = new Vector3 (transform.position.x + UnityEngine.Random.Range(1,5), transform.position.y, transform.position.z);
		} else {
			targetPosition = new Vector3 (transform.position.x - UnityEngine.Random.Range(1,5), transform.position.y, transform.position.z);
			agent.SetDestination (targetPosition);
		}
		attackIdleTimerSet = UnityEngine.Random.Range (1.0f, 6.0f);
		attackIdleTimer = attackIdleTimerSet;
	}

	void checkEnemyNo() {
		var player = GameObject.Find ("Sanzus").GetComponent<ThirdPersonLogic>();
		if (player.enemyNo < 3 && player.enemyRefList.Count > 0) {
			var playerAttack = GameObject.Find ("Sanzus").GetComponent<AttackManager>();
			if (!playerAttack.enemyUpdateDelay && player.enemyRefList.Count > 0) {
				//print ("pushed forward, " +player.enemyNo);
				pushForward();
			}
		}
		waitingTimer = waitingTimerSet;
	}
	
	void pushForward() {
		var player = GameObject.Find ("Sanzus").GetComponent<ThirdPersonLogic>();
		player.enemyRefList[0].GetComponent<AICharacterControl>().attackExt = false;
		player.enemyRefList[0].GetComponent<AICharacterControl>().attack = true;
		player.enemyRefList.RemoveAt (0);
		player.enemyNo++;
	}

	void runAttackRNG() {
		if (runRNG) {
			atkRNG = UnityEngine.Random.Range (1, 5);
			//print ("rng: " + atkRNG);
			if (atkRNG < 4) {
				RNGtimer = RNGtimerSet;
			}
			if (atkRNG == 4) {
				//print ("attack");
				LHand.GetComponent<EnemyAttackManager>().canHit = true;
				RHand.GetComponent<EnemyAttackManager>().canHit = true;
				if (GetComponent<EnemyLogic>().health <= 4) {
				    GetComponent<Animation>().Play("EnemyAttack2");
				} else {
					GetComponent<Animation>().Play("EnemyAttack");
				}
				runRNG = false;
				RNGtimer = RNGtimerSet;
				StartCoroutine (resetAttack (3.5f));
			}
		}
	}

	void wander () {
		wanderTimerSet = UnityEngine.Random.Range (1, 5);
		wanderTimer = wanderTimerSet;
		circleOffset = 5;
		circleRadius = UnityEngine.Random.Range (3, 10);
		Vector3 objPos = new Vector3(transform.position.x + (transform.forward.x*circleOffset), transform.position.y ,transform.position.z + (transform.forward.z*circleOffset));
		Vector3 source = Random.insideUnitSphere * circleRadius;
		targetPosition = new Vector3(source.x + objPos.x, objPos.y, source.z + objPos.z);
		agent.SetDestination (targetPosition);
		transform.LookAt (targetPosition);
		isWandering = true;
	}

	void turnAround () {
		targetPosition = GameObject.Find ("Enemy Spawn").transform.position;
		agent.SetDestination (targetPosition);
		transform.LookAt (targetPosition);
		hitBoundary = false;
		isWandering = true;
	}

	void returnToArea() {
		print ("returnnnnn");
		targetPosition = GameObject.Find ("Enemy Spawn").transform.position;
		agent.SetDestination (targetPosition);
		transform.LookAt (targetPosition);
	}

	IEnumerator resetAttack (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		LHand.GetComponent<EnemyAttackManager>().canHit = false;
		RHand.GetComponent<EnemyAttackManager>().canHit = false;
		runRNG = true;
	}

	public void SetTarget(Transform target) {
    	this.target = target;
    }
}