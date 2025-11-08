using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
	[SerializeField] private int ammoAmount = 5;
    public AudioSource ammoPickupSound;

    void OnTriggerEnter(Collider other)
    {
		// Check player collision
		if (other.CompareTag("Player"))
        {
			// Add ammo to the player
			if (AmmoManager.instance != null)
            {
                AmmoManager.instance.AddAmmo(ammoAmount);
            }
            ammoPickupSound.Play();
            Destroy(gameObject);
        }
    }
}
