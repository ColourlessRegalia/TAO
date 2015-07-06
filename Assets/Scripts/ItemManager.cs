using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ItemManager : MonoBehaviour {

	private bool canUse;
	private bool canItemSwitch;
	private int itemNo; //current item selected
	public int currItemNo; //number of items current selection has

	// Use this for initialization
	void Start () {
		itemNo = PlayerData.currItem;
		canUse = true;
		canItemSwitch = true;
		updateItem();
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName != "Base") {
			float q = CrossPlatformInputManager.GetAxis ("D-Pad Horizontal");
			if (q > 0) {
				if (canItemSwitch) {
					if (PlayerData.currItem <= 4) {
						PlayerData.currItem++;
						canItemSwitch = false;
						StartCoroutine (enableItemSwitch (0.5f));
					} 
					if (PlayerData.currItem > 4) {
						PlayerData.currItem = 1;
						canItemSwitch = false;
						StartCoroutine (enableItemSwitch (0.5f));
					}
				}
				updateItem ();
			} else if (q < 0) {
				if (canItemSwitch) {
					if (PlayerData.currItem >= 1) { 
						PlayerData.currItem--;
						canItemSwitch = false;
						StartCoroutine (enableItemSwitch (0.5f));
					}
					if (PlayerData.currItem < 1) {
						PlayerData.currItem = 4;
						canItemSwitch = false;
						StartCoroutine (enableItemSwitch (0.5f));
					}
				}
				updateItem ();
			}
			itemNo = PlayerData.currItem;
			
			float z = CrossPlatformInputManager.GetAxis ("D-Pad Vertical");
			if (z < 0) {
				if (canUse && currItemNo > 0) {
					usePotion ();
				}
			} else if (z > 0) {
				PlayerData.playerHealth -= 10;
			}
		}
	}

	IEnumerator enableUse (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		canUse = true;
	}
	
	IEnumerator enableItemSwitch (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		canItemSwitch = true;
	}
	
	void updateItem() {
		if (itemNo == 1) {
			currItemNo = PlayerData.item1No;
		}
		
		if (itemNo == 2) {
			currItemNo = PlayerData.item2No;
		}
		
		if (itemNo == 3) {
			currItemNo = PlayerData.item3No;
		}
		
		if (itemNo == 4) {
			currItemNo = PlayerData.item4No;
		}
	}
	
	void usePotion() { //using potions
		if (itemNo == 1) {
			PlayerData.item1No--;
			PlayerData.playerHealth += 15;
		}
		
		if (itemNo == 2) {
			PlayerData.item2No--;
			PlayerData.playerHealth += 20;
		}
		
		if (itemNo == 3) {
			PlayerData.item3No--;
			PlayerData.playerHealth += 50;
		}
		
		if (itemNo == 4) {
			PlayerData.item4No--;
			PlayerData.playerHealth += 5;
		}
		updateItem();
		canUse = false;
		StartCoroutine(enableUse(0.75f));
	}
}