using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	[Header("Mouvement")]
    public float moveSpeed = 20f;

    [Header("Caméra")]
    [SerializeField] public float mouseSensitivity = 200f;
    [SerializeField] public Transform playerBody;
    [SerializeField] public Transform shootPoint;
    // private HealthBar health;
    

    private float xRotation = 0f;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	void Start()
    {
		// Verrouille le curseur au centre de
		Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        // --- Mouvement du Player ---
        float moveX = Input.GetAxis("Horizontal"); // A/D ou flèches gauche/droite
        float moveZ = Input.GetAxis("Vertical");   // W/S ou flèches haut/bas

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.position += move * moveSpeed * Time.deltaTime;

        // --- Rotation de la caméra ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Empêche la caméra de tourner à 360° verticalement

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        shootPoint.rotation = Camera.main.transform.rotation;

        playerBody.Rotate(Vector3.up * mouseX);
    }

	public void TakeDamage(int damage)
    {
        HealthBar.instance.takeDammage(damage);
	}
}

