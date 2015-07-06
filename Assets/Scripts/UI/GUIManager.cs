using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public Slider healthBarSlider;
	public Slider manaBarSlider;
	public Animator elementWheel;
	public Animator itemBar;
	public Text itemText;
	public Sprite fireMana;
	public Sprite iceMana;
	public Sprite windMana;
	public Sprite earthMana;
	public GameObject FireGlow;
	public GameObject IceGlow;
	public GameObject WindGlow;
	public GameObject EarthGlow;
	
	void Start () {
		elementWheel.Play ("WheelFireIdle");
		checkItemState ();
	}

	private void checkItemState () {
		if (PlayerData.currItem == 1) {
			itemBar.Play("ItemBarIdle1");
		}

		if (PlayerData.currItem == 2) {
			itemBar.Play("ItemBarIdle2");
		}

		if (PlayerData.currItem == 3) {
			itemBar.Play("ItemBarIdle3");
		}

		if (PlayerData.currItem == 4) {
			itemBar.Play("ItemBarIdle4");
		}
	}

	IEnumerator resetFire (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		elementWheel.SetBool("fireToIce", false);
		elementWheel.SetBool("fireToEarth", false);
	}

		IEnumerator resetIce (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		elementWheel.SetBool("iceToWind", false);
		elementWheel.SetBool("iceToFire", false);
	}

	IEnumerator resetWind (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		elementWheel.SetBool("windToEarth", false);
		elementWheel.SetBool("windToIce", false);
	}

	IEnumerator resetEarth (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		elementWheel.SetBool("earthToFire", false);
		elementWheel.SetBool("earthToWind", false);
	}

	IEnumerator resetItemOne (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		itemBar.SetBool ("4to1", false);
		itemBar.SetBool ("2to1", false);
	}

	IEnumerator resetItemTwo (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		itemBar.SetBool ("1to2", false);
		itemBar.SetBool ("3to2", false);
	}

	IEnumerator resetItemThree (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		itemBar.SetBool ("2to3", false);
		itemBar.SetBool ("4to3", false);
	}

	IEnumerator resetItemFour (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		itemBar.SetBool ("3to4", false);
		itemBar.SetBool ("1to4", false);
	}

//	IEnumerator rotateFire(float rotationTime){
//		print (rotationTime);
//		var elementWheel2 = GameObject.Find("ElementWheel");
//		float rotZ = elementWheel2.GetComponent<RectTransform>().rotation.eulerAngles.z;
//		rotZ += 90f;
//		Quaternion elemenetWheel2Ori = elementWheel2.GetComponent<RectTransform>().rotation;
//		Quaternion elementWheel2Rot = Quaternion.Euler(elementWheel2.GetComponent<RectTransform>().rotation.eulerAngles.x,	
//		                                               elementWheel2.GetComponent<RectTransform>().rotation.eulerAngles.y,
//		                                               rotZ);
//		elementWheel2.GetComponent<RectTransform>().rotation = Quaternion.Slerp(elemenetWheel2Ori, elementWheel2Rot, rotationTime);
//		yield return new WaitForSeconds (rotationTime);
//	}

	private void Update () {
		var cube = GameObject.Find ("Sanzus").transform.FindChild ("Attack Cube");
//		var attackManager = GameObject.Find ("Sanzus").GetComponent<AttackManager> ();
		itemText.text = GameObject.Find ("Sanzus").GetComponent<ItemManager> ().currItemNo.ToString ();
		if (CrossPlatformInputManager.GetButtonDown ("Element +")/* && attackManager.elementPause == false*/) {
			if (cube.GetComponent<MeshRenderer> ().material.name == "FireAtkCubeMat (Instance)") {
				elementWheel.SetBool("fireToIce", true);
				StartCoroutine (resetFire (0.13f));
			}
			if (cube.GetComponent<MeshRenderer> ().material.name == "IceAtkCubeMat (Instance)") {
				elementWheel.SetBool("iceToWind", true);
				StartCoroutine (resetIce (0.13f));
			}
			if (cube.GetComponent<MeshRenderer> ().material.name == "WindAtkCubeMat (Instance)") {
				elementWheel.SetBool("windToEarth", true);
				StartCoroutine (resetWind (0.13f));
			}
			if (cube.GetComponent<MeshRenderer> ().material.name == "EarthAtkCubeMat (Instance)") {
				elementWheel.SetBool("earthToFire", true);
				StartCoroutine (resetEarth (0.13f));
			}
		}

		if (CrossPlatformInputManager.GetButtonDown ("Element -")/* && attackManager.elementPause == false*/) {
			if (cube.GetComponent<MeshRenderer> ().material.name == "FireAtkCubeMat (Instance)") {
				elementWheel.SetBool("fireToEarth", true);
				StartCoroutine (resetFire (0.13f));
			}
			if (cube.GetComponent<MeshRenderer> ().material.name == "IceAtkCubeMat (Instance)") {
				elementWheel.SetBool("iceToFire", true);
				StartCoroutine (resetIce (0.13f));
			}
			if (cube.GetComponent<MeshRenderer> ().material.name == "WindAtkCubeMat (Instance)") {
				elementWheel.SetBool("windToIce", true);
				StartCoroutine (resetWind (0.13f));
			}
			if (cube.GetComponent<MeshRenderer> ().material.name == "EarthAtkCubeMat (Instance)") {
				elementWheel.SetBool("earthToWind", true);
				StartCoroutine (resetEarth (0.13f));
			}
		}

		float q = CrossPlatformInputManager.GetAxis ("D-Pad Horizontal");
		if (q > 0) { //next item on the list
			if (PlayerData.currItem == 1) {
				itemBar.SetBool("4to1", true);
				StartCoroutine (resetItemOne (0.13f));
			}
			if (PlayerData.currItem == 2) {
				itemBar.SetBool("1to2", true);
				StartCoroutine (resetItemTwo (0.13f));
			}
			if (PlayerData.currItem == 3) {
				itemBar.SetBool("2to3", true);
				StartCoroutine (resetItemThree (0.13f));
			}
			if (PlayerData.currItem == 4) {
				itemBar.SetBool("3to4", true);
				StartCoroutine (resetItemFour (0.13f));
			}
		} else if (q < 0) { //previous item
			if (PlayerData.currItem == 1) {
				itemBar.SetBool("2to1", true);
				StartCoroutine (resetItemOne (0.13f));
			}
			if (PlayerData.currItem == 2) {
				itemBar.SetBool("3to2", true);
				StartCoroutine (resetItemTwo (0.13f));
			}
			if (PlayerData.currItem == 3) {
				itemBar.SetBool("4to3", true);
				StartCoroutine (resetItemThree (0.13f));
			}
			if (PlayerData.currItem == 4) {
				itemBar.SetBool("1to4", true);
				StartCoroutine (resetItemFour (0.13f));
			}
		}

		healthBarSlider.value = PlayerData.playerHealth;
		if (healthBarSlider.value <= 0) {
			healthBarSlider.fillRect.gameObject.SetActive (false);
		} else {
			healthBarSlider.fillRect.gameObject.SetActive (true);
		}

		manaBarSlider.value = PlayerData.playerMana;
		if (manaBarSlider.value <= 0) {
			manaBarSlider.fillRect.gameObject.SetActive (false);
		} else {
			manaBarSlider.fillRect.gameObject.SetActive (true);
		}

		if (cube.GetComponent<MeshRenderer> ().material.name == "FireAtkCubeMat (Instance)") {
			manaBarSlider.fillRect.gameObject.GetComponent<Image>().sprite = fireMana;
			FireGlow.SetActive (true);
			IceGlow.SetActive (false);
			WindGlow.SetActive (false);
			EarthGlow.SetActive (false);
		}

		if (cube.GetComponent<MeshRenderer> ().material.name == "IceAtkCubeMat (Instance)") {
			manaBarSlider.fillRect.gameObject.GetComponent<Image>().sprite = iceMana;
			FireGlow.SetActive (false);
			IceGlow.SetActive (true);
			WindGlow.SetActive (false);
			EarthGlow.SetActive (false);
		}

		if (cube.GetComponent<MeshRenderer> ().material.name == "WindAtkCubeMat (Instance)") {
			manaBarSlider.fillRect.gameObject.GetComponent<Image>().sprite = windMana;
			FireGlow.SetActive (false);
			IceGlow.SetActive (false);
			WindGlow.SetActive (true);
			EarthGlow.SetActive (false);
		}

		if (cube.GetComponent<MeshRenderer> ().material.name == "EarthAtkCubeMat (Instance)") {
			manaBarSlider.fillRect.gameObject.GetComponent<Image>().sprite = earthMana;
			FireGlow.SetActive (false);
			IceGlow.SetActive (false);
			WindGlow.SetActive (false);
			EarthGlow.SetActive (true);
		}
	}
}