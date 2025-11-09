using UnityEngine;

public class BowShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;

    private float minPower = 10f;
    private float maxPower = 60f;
    private float chargeSpeed = 50f;

    private float currentPower;
    private bool isCharging = false;
    
    public Camera playerCamera;
    [SerializeField] private float zoomFOV = 40f;   // FOV when aiming
    [SerializeField] private float defaultFOV = 70f;
    
    void Update()
    {


        // If the player starts pressing (left click)
        if (Input.GetButtonDown("Fire1") && (PauseMenuManager.instance == null || !PauseMenuManager.instance.isPaused))
        {

            // Check that there are arrows left
            if (AmmoManager.instance != null && !AmmoManager.instance.UseAmmo())
                return; // no shot if out of ammo
            isCharging = true;
            currentPower = minPower;
        }

        // While holding the click, charge the power
        if (isCharging && Input.GetButton("Fire1"))
        {   
            currentPower += chargeSpeed * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, minPower, maxPower);
            playerCamera.fieldOfView = Mathf.Lerp(defaultFOV, zoomFOV, ((currentPower - minPower) / (maxPower - minPower)));
        }

        // When released, shoot
        if (isCharging && Input.GetButtonUp("Fire1"))
        {
            ShootArrow();
            isCharging = false;
            playerCamera.fieldOfView = defaultFOV;
        }
        
    }

    void ShootArrow()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        
        if (Physics.Raycast(ray,out RaycastHit hit, 500f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(500f);
        }
        Vector3 shootDir = (targetPoint - shootPoint.position).normalized;
        
        // Instantiate the arrow at the ShootPoint position
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.LookRotation(shootDir));

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        // Apply initial velocity according to the power
        rb.AddForce(shootDir * currentPower, ForceMode.Impulse);


        // Pass the power value for damage if needed
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            float chargeRatio = Mathf.InverseLerp(minPower, maxPower, currentPower);
            arrowScript.SetDamage(chargeRatio);
        }        
    }
}
