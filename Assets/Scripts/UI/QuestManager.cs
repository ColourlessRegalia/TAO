using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class QuestManager : MonoBehaviour {
	private bool canOpenQuest;
	public GameObject worldMap;
	public GameObject exitButton;
	
	void Start () {
		canOpenQuest = false;
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			canOpenQuest = true;
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.tag == "Player") {
			canOpenQuest = false;
		}
	}

	void Update () {
		if(CrossPlatformInputManager.GetButtonDown("Horizontal")){

		}

		if (CrossPlatformInputManager.GetButtonDown ("Submit")) {
			if (canOpenQuest) {
				showMap();
			}
		}

		if (worldMap.activeInHierarchy) { //disable character movement when map is up
			GameObject.Find ("Sanzus").GetComponent<ThirdPersonLogic> ().isAttacking = true;
		} else {
			GameObject.Find("Sanzus").GetComponent<ThirdPersonLogic>().isAttacking = false;
		}
	}
	
	void showMap() {
		worldMap.SetActive(true);
		exitButton.SetActive(true);
	}

	public void hideMap() { //hides map 
		worldMap.SetActive(false);
		exitButton.SetActive (false);
	}
	
	public void Outskirt() {
		PlayerData.spawnPosition = new Vector3(-18.83f, -0.35f, -20.27f);
		PlayerData.spawnRotation = new Vector3(0f, 45f, 0f);
		Application.LoadLevel("Outskirt");
	}
}