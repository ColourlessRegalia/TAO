using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

	private ThirdPersonLogic Sanzus; //Storing the Player's script as Sanzus.
	public int boxHealth; //The boxHealth, amount of times being hit before being destroyed.
	public GameObject[] itemsDrop; //List of item to drop.
	private Vector3 itemDropPosition; // Position where the item will drop.
    private ParticleSystem particleSystem;

	void Start(){

		//Find the gameObject with the tag Player and storing its component(Script, ThirdPersonLogic) as Sanzus.
		Sanzus = GameObject.FindGameObjectWithTag ("Player").GetComponent<ThirdPersonLogic> ();

        //Get the particle system of the script Holder(Crate/Box)
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();

        //Stop the particle system from playing.
        particleSystem.Stop();

		//Setting the boxHealh.
		boxHealth = 4; 

		//Settng thte itemDropPosition at the script Holder's position.
		itemDropPosition = transform.position;

		//Increasing the Y-axis value so the item doesn't sink into the ground.
		itemDropPosition.y += 0.5f;
	}

	void OnTriggerEnter(Collider player)
    {
		// If the player is attacking and its weapon enters(hit) the holder of the script.
		if (player.gameObject.tag == "Melee" && Sanzus.isAttacking) 
        {
			//Reduce its health by 1.
			boxHealth -= 1;

            //Play the particle system.
            particleSystem.Play();

			//Should the boxHealth falls below 0.
			if (boxHealth <= 0)
            {
				//Randomly generate an integer, note 2 will not be generate, reger to Unity random.Range manual.
				int randomItemGenerator = Random.Range(0, 2);

				// Should 1 be gernerated,
				if (randomItemGenerator == 1)
                {
					//Creates the item from the list of itemsDrop can be dropped
					Instantiate(itemsDrop[randomItemGenerator - 1], itemDropPosition , Quaternion.identity);
				}
				//Destroy the gameObject.
				Destroy (gameObject);
			}
		}
	}
}
