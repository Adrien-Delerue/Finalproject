using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 10f;
    private Rigidbody rb;
    private bool stuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // collisions pr�cises � haute vitesse
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        if (!stuck)
        {
            // La fl�che regarde toujours dans la direction du d�placement
            if (rb.velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
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

        // La fl�che se fixe � l'objet touch�
        rb.isKinematic = true; // plus de physique
        transform.parent = collision.transform; // s�attache � l�objet touch�
        stuck = true;
    }
}
