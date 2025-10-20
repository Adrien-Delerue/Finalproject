using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 25f;  // dégâts de base de la flèche
    public float lifetime = 5f; // durée avant destruction automatique

    private Rigidbody rb;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime); // détruit la flèche après un délai
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return; // empêche les collisions multiples
        hasHit = true;

        // Vérifie si l’objet touché a un script "Enemy"
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Optionnel : désactive la physique après impact
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        transform.parent = collision.transform; // colle la flèche dans la cible

        // Détruit la flèche après un court délai pour l’effet visuel
        Destroy(gameObject, 2f);
    }
}
