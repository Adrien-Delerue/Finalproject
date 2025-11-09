using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float maxDamage =20f;
    private float minDamge = 10f;
	public float damage;
	[SerializeField] private float lifetime = 5f;
    [SerializeField] private GameObject dustEffectPrefab;

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
		if (!hasHit && rb.velocity.sqrMagnitude > 0.0001f) {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
		// Ignore multiple collision triggers
		if (hasHit) return;
		hasHit = true;

        ContactPoint contact = collision.contacts[0];
        
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Freeze physics on impact
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

        }
        else
        {
            // Check for enemy hit
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Quaternion rot = Quaternion.identity;
                Instantiate(dustEffectPrefab, contact.point, rot);
            }
            
            Destroy(gameObject);
        }
    }
    public void SetDamage(float chargeRatio)
    {
        damage = Mathf.Lerp(minDamge, maxDamage, chargeRatio);
    }
}
