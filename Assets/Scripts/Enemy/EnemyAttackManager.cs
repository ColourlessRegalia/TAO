using UnityEngine;
using System.Collections;

public class EnemyAttackManager : MonoBehaviour {

	private int damage;
	public bool canHit;

	// Use this for initialization
	void Start () {
		damage = 5;
		canHit = false;
	}
	
	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Player" && canHit) {
			PlayerData.playerHealth -= damage;
			var player = GameObject.Find ("Sanzus").GetComponent<ThirdPersonLogic>();
			player.isHit = true;
		}
	}
}