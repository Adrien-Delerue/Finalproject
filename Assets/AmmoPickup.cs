using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 5; // combien de fl�ches �a donne

    void OnTriggerEnter(Collider other)
    {
        // V�rifie que c'est le joueur
        if (other.CompareTag("Player"))
        {
            // Ajoute les munitions
            if (AmmoManager.instance != null)
            {
                AmmoManager.instance.AddAmmo(ammoAmount);
            }
            Debug.Log("ammo pickup");
            // D�truit l'objet ramass�
            Destroy(gameObject);
        }
    }
}
