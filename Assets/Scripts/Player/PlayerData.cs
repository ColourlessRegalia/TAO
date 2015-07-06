using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	private static PlayerData instanceRef;
	public static int playerHealth;
	public static int playerMana;
	public static int currItem;
	public static int item1No;
	public static int item2No;
	public static int item3No;
	public static int item4No;
	public static int enemyList;
	//public static int lastElement;
	public static Vector3 spawnPosition;
	public static Vector3 spawnRotation;

	// Use this for initialization
	void Awake() {
		if (instanceRef == null) {
			instanceRef = this;
			DontDestroyOnLoad (gameObject);
		} else {
			DestroyImmediate (gameObject);
		} 

		currItem = 1;
		item1No = 5;
		item2No = 9;
		item3No = 2;
		item4No = 14;
		enemyList = 0;
	}

	void Start () {
		playerHealth = 100;
		playerMana = 100;
	}

	void Update() {
		if (Application.loadedLevelName == "Base") {
			Destroy(this.gameObject);
		}

		if (playerHealth > 100) {
			playerHealth = 100;
		} else if (playerHealth < 0) {
			playerHealth = 0;
			spawnPosition = new Vector3(0,0,0);
			spawnRotation = new Vector3 (0,0,0);
			Application.LoadLevel("Base");
		}

		if (playerMana > 100) {
			playerMana = 100;
		} else if (playerMana < 0) {
			playerMana = 0;
		}

		if (Input.GetKeyDown (KeyCode.V)) {
			//Application.LoadLevel("Base");
			//Destroy(this.gameObject);
			//playerHealth -= 10;
		}
		//blaaaah
	}
}