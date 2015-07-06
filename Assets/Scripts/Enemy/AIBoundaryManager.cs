using UnityEngine;
using System.Collections;

public class AIBoundaryManager : MonoBehaviour {

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Enemy") {
			if (collider.GetComponent<AICharacterControl>() != null) {
				if (collider.GetComponent<AICharacterControl>().attack == true || collider.GetComponent<AICharacterControl>().attackExt == true || collider.GetComponent<AICharacterControl>().flee == true) {
					//nooothing
				} else {
					collider.GetComponent<AICharacterControl>().hitBoundary = true;
				}

				if (collider.GetComponent<AICharacterControl>().returnToSpawn) {
					collider.GetComponent<AICharacterControl>().hitBoundary = false;
				}
			} else if  (collider.GetComponent<AIRangedCharacterControl>() != null) {
				if (collider.GetComponent<AIRangedCharacterControl>().attack == true || collider.GetComponent<AIRangedCharacterControl>().flee == true || collider.GetComponent<AIRangedCharacterControl>().evade == true) {
					//nooothing
				} else {
					collider.GetComponent<AIRangedCharacterControl>().hitBoundary = true;
				}
				
				if (collider.GetComponent<AIRangedCharacterControl>().returnToSpawn) {
					collider.GetComponent<AIRangedCharacterControl>().hitBoundary = false;
				}
			}
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.gameObject.tag == "Enemy") {
			if (collider.GetComponent<AICharacterControl>() != null) {
				if (collider.GetComponent<AICharacterControl>().returnToSpawn) {
					collider.GetComponent<AICharacterControl>().returnToSpawn = false;
					collider.GetComponent<AICharacterControl>().isWandering = true;
				} else {
					if (collider.GetComponent<AICharacterControl>().attack == true || collider.GetComponent<AICharacterControl>().attackExt == true || collider.GetComponent<AICharacterControl>().flee == true) {
						collider.GetComponent<AICharacterControl>().returnToSpawn = true;
					}
				}
			} else if (collider.GetComponent<AIRangedCharacterControl>() != null) {
				if (collider.GetComponent<AIRangedCharacterControl>().returnToSpawn) {
					collider.GetComponent<AIRangedCharacterControl>().returnToSpawn = false;
					collider.GetComponent<AIRangedCharacterControl>().isWandering = true;
				} else {
					if (collider.GetComponent<AIRangedCharacterControl>().attack == true || collider.GetComponent<AIRangedCharacterControl>().flee == true || collider.GetComponent<AIRangedCharacterControl>().evade == true) {
						collider.GetComponent<AIRangedCharacterControl>().returnToSpawn = true;
					}
				}
			}
		}
	}
}