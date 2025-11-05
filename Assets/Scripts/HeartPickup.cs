using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int hpAmount = 10;
    [SerializeField] private AudioClip pickupSound; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthBar.instance != null)
            {
                HealthBar.instance.giveHP(hpAmount);
                Debug.Log("HP pickup: +" + hpAmount);
            }
            else
            {
                Debug.LogWarning("HealthBar.instance is null!");
            }

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 50f * Time.deltaTime);
    }
}