using UnityEngine;

public class BowZoom : MonoBehaviour
{
    public Camera playerCamera;   // caméra du joueur
    [SerializeField] public float zoomFOV = 10f;   // FOV quand on vise
    [SerializeField] public float zoomSpeed = 5f;  // vitesse de transition
    [SerializeField] public float defaultFOV=50f;     // FOV normal

    [SerializeField] public bool isCharging = false; // le BowShoot mettra à true quand on charge

    void Start()
    {
        if (playerCamera == null)
        defaultFOV = playerCamera.fieldOfView;
    }

    void Update()
    {
        float targetFOV = isCharging ? zoomFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
