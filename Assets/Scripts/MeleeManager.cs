using UnityEngine;
using System.Collections;

public class MeleeManager : MonoBehaviour {

private int damage;
//private ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		damage = 2;
        //particleSystem = GetComponentInChildren<ParticleSystem>();
        //particleSystem.Stop();
	}
	
	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Enemy") {   
			if (GameObject.Find("Sanzus").GetComponent<ThirdPersonLogic>().isAttacking) {
				var h = collider.GetComponent<EnemyLogic> ();
				if (h.isHit == false) {
				//	collider.GetComponent<Animation> ().Play ("EnemyHurt");
					h.health -= damage;
                    //particleSystem.transform.position = collider.transform.position;
                    //particleSystem.Play();
				}
			}
		}
//
//		if (collider.gameObject.tag == "Crate") {
//			if (GameObject.Find("Sanzus").GetComponent<ThirdPersonLogic>().isAttacking) {
//				Destroy(collider.gameObject);
//			}
//		}
	}
}