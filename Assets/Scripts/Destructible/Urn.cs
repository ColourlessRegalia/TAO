using UnityEngine;
using System.Collections;

public class Urn : MonoBehaviour {

	public GameObject healthPotion;  //The health potion where the object will drop.
	private ThirdPersonLogic Sanzus; //Storing the Player's script as Sanzus.
	private Vector3 itemDropPosition; //Position where the item will drop.

	void Start(){
		//Finding the gameobject with the tag Player and getting its script(component, ThirdPersonLogic).
		Sanzus = GameObject.FindGameObjectWithTag ("Player").GetComponent<ThirdPersonLogic>();

		//Storing the urnPosition as the holder's position.
		itemDropPosition = transform.position; 

		//Increaing the Y position so the itemDrop doesn't sink into the ground.
		itemDropPosition.y += 0.5f;

	}

	void OnTriggerEnter(Collider player){

		// Should the player attack and it's melee weapon enters,
		if (player.gameObject.tag == "Melee" && Sanzus.isAttacking) {

			//The urn drops the item at the urn position and rotation.
			Instantiate(healthPotion, itemDropPosition, Quaternion.identity);

			//Destroy the gameObject, the urn.
			Destroy(gameObject);
		}
	}
}
