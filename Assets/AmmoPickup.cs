using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 5; // combien de flèches ça donne

    void OnTriggerEnter(Collider other)
    {
        // Vérifie que c'est le joueur
        if (other.CompareTag("Player"))
        {
            // Ajoute les munitions
            if (AmmoManager.instance != null)
            {
                AmmoManager.instance.AddAmmo(ammoAmount);
            }
            Debug.Log("ammo pickup");
            // Détruit l'objet ramassé
            Destroy(gameObject);
        }
    }
}
