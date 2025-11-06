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
        stuck = true;

        ContactPoint contact = collision.contacts[0];

        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.isKinematic = true;
            transform.parent = collision.transform;

            Destroy(gameObject, 5f);
        }
        else
        {// touch an ennemy
            var enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
