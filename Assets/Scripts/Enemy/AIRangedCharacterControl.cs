using System.Collections;
using UnityEngine;

[RequireComponent(typeof (NavMeshAgent))]
public class AIRangedCharacterControl : MonoBehaviour
{
	public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
	public GameObject bullet;
	public Transform target; // target to aim for
	public GameObject aim;
	private Vector3 steeringDirection;
	private Vector3 targetPosition;
	private float speed;
	private float detectAreaMax; //max detect distance
	private float detectAreaMin; //min detect distance
	private float detectAreaFlee; //distance where it flees
	private float RNGtimer;
	private float RNGtimerSet;
	private float wanderTimer;
	private float wanderTimerSet;
	private float runningTimer;
	private float runningTimerSet;
	private float circleOffset;
	private float circleRadius;
	private int atkRNG;
	private int listPos;
	private bool enableWandering;
	private bool canEvade;
	private bool move;
	private bool runRNG;
	public bool flee; //fleeing when alone
	public bool evade; //fleeing when player goes within fleeing range (regardless of enemy numbers)
	public bool isWandering;
	public bool attack;
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
		detectAreaMax = 150f;
		detectAreaMin = 50f;
		detectAreaFlee = 120f;
		RNGtimerSet = 1.0f;
		RNGtimer = RNGtimerSet;
		runningTimerSet = 3.0f;
		runningTimer = runningTimerSet;
		attack = false;
		flee = false;
		evade = false;
		canEvade = true;
		runRNG = true;
		enableWandering = true;
		isWandering = false;
		hitBoundary = false;
		returnToSpawn = false;
		agent.SetDestination (target.position);
	}

	// Update is called once per frame
	void Update() {
		if (move) {
			//print ("move");
			transform.LookAt(target);
			agent.SetDestination(transform.position+=Vector3.zero);
			//agent.SetDestination (target.position);
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
				if (!attack & !flee & !evade) {
					attack = true; 
					move = true;
					enableWandering = false;
				}
			}
		} else {
			move = false;
			enableWandering = true;
		}
		
		if (spawn.enemyCount <= 1) {
			if (attack) {
				attack = false;
			}
			flee = true;
			if ((target.position - transform.position).sqrMagnitude < detectAreaFlee) {
				transform.LookAt (target);
				transform.rotation = Quaternion.Inverse (target.rotation);
				Vector3 moveDir = transform.position - player.transform.position;
				transform.position += moveDir.normalized * speed * Time.deltaTime;
			} else {
				move = false;
				flee = false;
				enableWandering = true;
			}
		} else {
			flee = false;
		}
		
		if (attack) { 
			if ((target.position - transform.position).sqrMagnitude > detectAreaMax && !evade) {
				attack = false;
				move = false;
				enableWandering = true;
			} else if ((target.position - transform.position).sqrMagnitude < detectAreaMax && !evade) {
				transform.LookAt (target);
				move = false;
				enableWandering = false;
				if (RNGtimer > 0) {
					if (player.isHit == false) {
						RNGtimer -= Time.deltaTime;
					}
				}
				if (RNGtimer <= 0) {
					runAttackRNG ();
				}
			}
		}

		if ((target.position - transform.position).sqrMagnitude < detectAreaMin && !flee) {
			if (!evade) {
				if (canEvade) {
					attack = false;
					print ("evade");
					evade = true;
				}
			} 
		}

		if (evade) {
			if (runningTimer > 0) {
				transform.LookAt(target);
				transform.rotation = Quaternion.Inverse(target.rotation);
				Vector3 moveDir = transform.position - player.transform.position;
				transform.position += moveDir.normalized * speed * Time.deltaTime;
				runningTimer -= Time.deltaTime;
			} else if (runningTimer <= 0) {
				print ("stop evading");
				stopRun();
			}
		}
		
		if (hitBoundary) {
			wanderTimer = wanderTimerSet;
			isWandering = false;
			turnAround();
		}
		
		if (returnToSpawn) {
			if (!attack && !evade && !flee) {
				isWandering = false;
				returnToArea ();
			}
		} 
	}

	void stopRun() {
		evade = false;
		canEvade = false;
		if (attack) {
			attack = false;
		}
		runningTimer = runningTimerSet;
		StartCoroutine (canMove (3.0f));
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
				GetComponent<Animation>().Play("EnemyAttack");
				spawnBullet();
				runRNG = false;
				RNGtimer = RNGtimerSet;
				StartCoroutine (resetAttack (3.5f));
			}
		}
	}

	void spawnBullet() {
		Instantiate (bullet, aim.transform.position + 1.0f * transform.forward, transform.rotation);
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
		targetPosition = GameObject.Find ("Enemy Spawn").transform.position;
		agent.SetDestination (targetPosition);
		transform.LookAt (targetPosition);
	}
	
	IEnumerator resetAttack (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		runRNG = true;
	}

	IEnumerator canMove (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		print ("can evade");
		canEvade = true;
		attack = true;
	}
	
	public void SetTarget(Transform target) {
		this.target = target;
	}
}