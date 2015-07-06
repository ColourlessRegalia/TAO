using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerAttack : MonoBehaviour {

	public bool isAttacking;

	private int attackState;

	enum ElementState {fire, ice, earth, wind};
	ElementState elementState;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
		{
		if (CrossPlatformInputManager.GetButtonDown ("Slash")) 
		{
			attackState ++;
			AttackAnimation ();
		}
	}

	void AttackAnimation(){

		isAttacking = true;

		if (isAttacking) {
			switch (elementState) {
			case ElementState.fire:
				switch (attackState) {

				case 1:
							//GetComponent<Animation>().Play("FireAttack01");
							StopCoroutine ("FinishAttack");
							StartCoroutine ("FinishAttack", (GetComponent<Animation> () ["FireAttack01"].clip.length + 0.23f));
					break;
						
				case 2:
							//GetComponent<Animation>().Play("FireAttack02");
							StopCoroutine("FinishAttack");
							StartCoroutine("FinishAttack",(GetComponent<Animation>()["FireAttack02"].clip.length + 0.23f));
					break;

				case 3:
							//GetComponent<Animation>().Play("FireAttack03");
							StopCoroutine("FinishAttack");
							StartCoroutine("FinishAttack",(GetComponent<Animation>()["FireAttack03"].clip.length + 0.23f));
					break;

				}
				break;
					
			case ElementState.ice:
				switch (attackState) {
				case 1:
					break;
							
				case 2:
					break;
							
				case 3:
							
					break;
							
				}
				break;
			}
		}
	}
	IEnumerator FinishAttack(float waitTime){
		yield return new WaitForSeconds (waitTime);
		isAttacking = false;
		attackState = 0;
	}
}
