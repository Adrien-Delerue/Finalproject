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
    
    public Camera playerCamera;
    [SerializeField] private float zoomFOV = 40f;   // FOV when aiming
	[SerializeField] private float zoomSpeed = 2f;  // transition speed
    [SerializeField] private float defaultFOV = 50f;

    void Update()
    {
        float targetFOV = isCharging ? zoomFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        
        // If the player starts pressing (left click)
        if (Input.GetButtonDown("Fire1"))
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
        }

        // When released, shoot
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
        
        if (Physics.Raycast(ray,out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
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
            arrowScript.damage = currentPower;
        }        
    }
}
