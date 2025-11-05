using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int hpAmount = 10;
    [SerializeField] private AudioClip pickupSound; // Optionnel

    void OnTriggerEnter(Collider other)
    {
        // Vérifie que c'est le joueur
        if (other.CompareTag("Player"))
        {
            // Ajoute les HP
            if (HealthBar.instance != null)
            {
                HealthBar.instance.giveHP(hpAmount);
                Debug.Log("HP pickup: +" + hpAmount);
            }
            else
            {
                Debug.LogWarning("HealthBar.instance is null!");
            }

            // Jouer un son (optionnel)
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Détruit le cœur ramassé
            Destroy(gameObject);
        }
    }

    // Optionnel : rotation du cœur pour le rendre plus visible
    void Update()
    {
        transform.Rotate(Vector3.up, 50f * Time.deltaTime);
    }
}