using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int hpAmount = 10;
    [SerializeField] private AudioSource pickupSound; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthBar.instance != null)
            {
                HealthBar.instance.giveHP(hpAmount);
            }
            else
            {
                Debug.LogWarning("HealthBar.instance is null!");
            }

            if (pickupSound != null)
            {
                pickupSound.Play();
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 50f * Time.deltaTime);
    }
}