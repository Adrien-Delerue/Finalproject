using UnityEngine;

public class BowShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;

    private float minPower = 10f;
    private float maxPower = 30f;
    private float chargeSpeed = 10f;

    private float currentPower;
    private bool isCharging = false;
    
    public Camera playerCamera;   // caméra du joueur
    [SerializeField] public float zoomFOV = 40f;   // FOV quand on vise
    [SerializeField] public float zoomSpeed = 2f;  // vitesse de transition
    [SerializeField] public float defaultFOV = 50f;     // FOV normal

    void Update()
    {
        float targetFOV = isCharging ? zoomFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        // Si le joueur commence à appuyer (clic gauche)
        if (Input.GetButtonDown("Fire1"))
        {
            // Vérifie qu'il reste des flèches
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

        // Quand il relâche, on tire
        if (isCharging && Input.GetButtonUp("Fire1"))
        {
            ShootArrow();
            isCharging = false;

        }
    }

    void ShootArrow()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        
        if(Physics.Raycast(ray,out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }
        Vector3 shootDir = (targetPoint - shootPoint.position).normalized;
        
        // Instancie la flèche à l'emplacement du ShootPoint
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.LookRotation(shootDir));

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        // Applique la vitesse initiale selon la puissance
        rb.AddForce(shootDir * currentPower, ForceMode.Impulse);


        // Transmet la puissance pour les dégâts si nécessaire
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.damage = currentPower;
        }        
    }
}
