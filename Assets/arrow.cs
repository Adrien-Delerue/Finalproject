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

    void Update()
    {
        if (!stuck)
        {
            // La fl�che regarde toujours dans la direction du d�placement
                transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (stuck) return;
        stuck = true;

       
        // Touche un ennemi
        var enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
