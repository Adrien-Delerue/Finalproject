using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 25f;  // d�g�ts de base de la fl�che
    public float lifetime = 5f; // dur�e avant destruction automatique

    private Rigidbody rb;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime); // d�truit la fl�che apr�s un d�lai
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return; // emp�che les collisions multiples
        hasHit = true;

        // V�rifie si l�objet touch� a un script "Enemy"
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Optionnel : d�sactive la physique apr�s impact
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        transform.parent = collision.transform; // colle la fl�che dans la cible

        // D�truit la fl�che apr�s un court d�lai pour l�effet visuel
        Destroy(gameObject, 2f);
    }
}
