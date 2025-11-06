using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager instance;

	[SerializeField] private int maxAmmo = 5;
    public int currentAmmo;

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
		if (amount <= 0) return;

		currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }

	// Called a BowShoot when fired
	public bool UseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            return true;
        }

        return false;
    }
}
