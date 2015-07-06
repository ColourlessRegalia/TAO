using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	private float spawnTimer;
	private float spawnTimerMax;
	public int enemyCount;
	public int enemyCountMax;
	public GameObject enemy; //enemy prefab 

	// Use this for initialization
	void Start () {
//		enemyCount = 0;
		spawnTimerMax = 3f;
		spawnTimer = spawnTimerMax;

        //if (Application.loadedLevelName == "Outskirt") {
        //    enemyCountMax = 3;
        //}

        //if (Application.loadedLevelName == "Volcano") {
        //    enemyCountMax = 5;
        //}
	}
	
	// Update is called once per frame
	void Update () {
		if (spawnTimer > 0) {
			spawnTimer -= Time.deltaTime;
		} if (spawnTimer <= 0) {
			if (enemyCount < enemyCountMax) {
				enemyCount++;
				Instantiate(enemy, new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
				spawnTimer = spawnTimerMax;
			} else {
				spawnTimer = spawnTimerMax;
			}
		}
	}
}
