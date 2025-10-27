using UnityEngine;

public class BowShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;

    private float minPower = 10f;
    private float maxPower = 50f;
    private float chargeSpeed = 10f;

    private float currentPower;
    private bool isCharging = false;
    
    public Camera playerCamera;   // cam�ra du joueur
    [SerializeField] public float zoomFOV = 40f;   // FOV quand on vise
    [SerializeField] public float zoomSpeed = 2f;  // vitesse de transition
    [SerializeField] public float defaultFOV = 50f;     // FOV normal

    void Update()
    {
        float targetFOV = isCharging ? zoomFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        // Si le joueur commence � appuyer (clic gauche)
        if (Input.GetButtonDown("Fire1"))
        {
            // V�rifie qu'il reste des fl�ches
            if (AmmoManager.instance != null && !AmmoManager.instance.UseAmmo())
                return; // pas de tir si plus de munitions
            isCharging = true;
            currentPower = minPower;
            
        }

        // Tant qu'il maintient le clic, on charge la puissance
        if (isCharging && Input.GetButton("Fire1"))
        {
            currentPower += chargeSpeed * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, minPower, maxPower);
        }

        // Quand il rel�che, on tire
        if (isCharging && Input.GetButtonUp("Fire1"))
        {
            ShootArrow();
            isCharging = false;

        }
    }

    void ShootArrow()
    {
        // Instancie la fl�che � l'emplacement du ShootPoint
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        // Direction : utilise le forward du ShootPoint (qui peut suivre la cam�ra)
        Vector3 shootDirection = shootPoint.forward;

        // Applique la vitesse initiale selon la puissance
        rb.velocity = shootDirection * currentPower;

        // Transmet la puissance pour les d�g�ts si n�cessaire
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.damage = currentPower;
        }
        
    }
}
