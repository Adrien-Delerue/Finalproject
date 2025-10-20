using UnityEngine;

public class BowShoot : MonoBehaviour
{
    [Header("Références")]
    public GameObject arrowPrefab;
    public Transform shootPoint;

    [Header("Paramètres de tir")]
    public float minPower = 5f;
    public float maxPower = 30f;
    public float chargeSpeed = 10f;

    private float currentPower;
    private bool isCharging = false;

    void Update()
    {
        // Si le joueur commence à appuyer (clic gauche)
        if (Input.GetButtonDown("Fire1"))
        {
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
        // Instancie la flèche à l'emplacement du ShootPoint
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        // Direction : utilise le forward du ShootPoint (qui peut suivre la caméra)
        Vector3 shootDirection = shootPoint.forward;

        // Applique la vitesse initiale selon la puissance
        rb.velocity = shootDirection * currentPower;

        // Transmet la puissance pour les dégâts si nécessaire
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.damage = currentPower;
        }
    }
}
