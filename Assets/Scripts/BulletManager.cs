using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

	private float speed;
	private float bulletTimer;

	// Use this for initialization
	void Start () {
		bulletTimer = 3f;
		speed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Time.deltaTime * speed * transform.forward;
		if (bulletTimer > 0) {
			bulletTimer -= Time.deltaTime;
		} if (bulletTimer <= 0) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			PlayerData.playerHealth -= 5;
			Destroy(this.gameObject);
		}
	}
}
