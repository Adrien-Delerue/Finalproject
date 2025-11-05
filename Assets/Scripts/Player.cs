using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	[Header("Mouvement")]
    public float moveSpeed = 5f;

    [Header("Caméra")]
    [SerializeField] public float mouseSensitivity = 100f;
    [SerializeField] public Transform playerBody;
    [SerializeField] public Transform shootPoint;
    
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
		// Lock the cursor to the center of the screen
		Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
		// Player Movement
		float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.position += move * moveSpeed * Time.deltaTime;

		// Camera Movement
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents the camera from rotating 360° vertically

		Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        shootPoint.rotation = Camera.main.transform.rotation;

        playerBody.Rotate(Vector3.up * mouseX);
    }

	public void TakeDamage(int damage)
    {
        HealthBar.instance.takeDammage(damage);
	}
}

