using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 10f;
    private Rigidbody rb;
    private bool stuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // collisions précises à haute vitesse
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (!stuck)
        {
            // La flèche regarde toujours dans la direction du déplacement
                transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (stuck) return;

        // Touche un ennemi
        var enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // La flèche se fixe à l'objet touché
        rb.isKinematic = true; // plus de physique
        transform.parent = collision.transform; // s’attache à l’objet touché
        stuck = true;
    }
}
