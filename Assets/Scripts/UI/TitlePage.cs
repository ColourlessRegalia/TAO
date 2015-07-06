using UnityEngine;
using System.Collections;

public class TitlePage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	public void newGame () {
		// Player initial position
		PlayerData.spawnPosition = new Vector3 (0f, 0f, 0f);
		// Player initial rotation
		PlayerData.spawnRotation = new Vector3 (0f, 0f, 0f);
		// Brining the player into the Base.
		Application.LoadLevel ("Base");
	}
}
