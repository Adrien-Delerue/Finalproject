using UnityEngine;

public class Arrow : MonoBehaviour
{
	public float damage = 10f;
	[SerializeField] private float lifetime = 5f;

	private Rigidbody rb;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

		// Enable continuous collision detection for fast-moving projectiles
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

		// Auto-destroy after lifetime expires
		Destroy(gameObject, lifetime);
    }

    void Update()
    {
		// If stuck to a moving target, follow its transform
		if (!hasHit) {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
		// Ignore multiple collision triggers
		if (hasHit) return;
		hasHit = true;

		// Freeze physics on impact
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.isKinematic = true;

		// Check for enemy hit
		Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
