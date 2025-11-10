using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Mouvement")]
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public float jumpHeight = 2f;
    [SerializeField] public float gravity = -15f;

    [Header("Caméra")]
    [SerializeField] public float mouseSensitivity = 0.5f;
    [SerializeField] public Transform playerBody;
    [SerializeField] public Transform shootPoint;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
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
        QualitySettings.vSyncCount = 0;
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Get or add CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // Player Movements
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Camera Movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        shootPoint.rotation = Camera.main.transform.rotation;
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void TakeDamage(int damage)
    {
        HealthBar.instance.takeDammage(damage);
    }
}