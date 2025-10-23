using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager instance;

    public int maxAmmo = 5;       // nombre maximum de flèches
    public int currentAmmo;        // flèches restantes


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo); // ne dépasse pas max
    }


    // Appelé depuis BowShoot quand on tire
    public bool UseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            return true; // tir possible
        }
        else
        {
            Debug.Log("Pas assez de flèches !");
            return false; // tir impossible
        }
    }
}
