using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {

	public int health;
	public bool isHit;

	// Use this for initialization
	void Start () {
		health = 7;
		isHit = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Animation> ().IsPlaying ("EnemyHurt")) {
			isHit = true;
		} else {
			isHit = false;
		}

		if (health <= 0) {
			var player = GameObject.Find ("ThirdPersonController").GetComponent<ThirdPersonLogic>();
			if (GetComponent<AICharacterControl>() != null) {
				if (GetComponent<AICharacterControl>().attack) {
					player.enemyNo--;
					if (player.enemyNo < 0) {
						player.enemyNo = 0;
					}
					print ("minus enemy, " +player.enemyNo);
				} else {
					if (GetComponent<AICharacterControl>().attackExt && player.enemyRefList.Count > 0) {
						for (int i =0; i < player.enemyRefList.Count; i++) {
							if (player.enemyRefList[i].gameObject.name == GetComponent<AICharacterControl>().enemyID) {
								player.enemyRefList.RemoveAt(i);
							}
						}
					}
				}
			}
			PlayerData.playerMana += 10;
			Destroy(this.gameObject);

			var spawn = GameObject.Find ("Enemy Spawn").GetComponent<EnemySpawner>();
			spawn.enemyCount--;
			if (spawn.enemyCount < 0) {
				spawn.enemyCount = 0;
			}
		}
	}
}