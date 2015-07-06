using UnityEngine;
using System.Collections;

public class FireSpecialAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("LifeTime", 2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator LifeTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
