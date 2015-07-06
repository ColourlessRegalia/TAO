using UnityEngine;
using System.Collections;

public class IceSpecialAttack : MonoBehaviour {

    public int damage;

    void OnParticleCollision(GameObject other)
    {
        print("ice hit");
        if (other.tag == "Enemy")
        {
            EnemyLogic enemyLogic = other.GetComponent<EnemyLogic>();
            enemyLogic.health -= damage;
        }
    }
}
