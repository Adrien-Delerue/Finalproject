using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float moveSpeed = 5f;

    [Header("Caméra")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform shootPoint;
    public int health = 100;

    private float xRotation = 0f;

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
}

