using UnityEngine;
using System.Collections;

public class HealthPotion : MonoBehaviour {
	
	public int healAmount; //amount the Player will heal
	
	void OnTriggerEnter(Collider player){
		//  If the holder of this script touches a gameObject tag as Player
		if (player.gameObject.tag == "Player") {

			// Increases the playerHealth by the healAmount.
			PlayerData.playerHealth += healAmount;

			//Destroy the current gameObject.
			Destroy(gameObject);
		}
	}
}
