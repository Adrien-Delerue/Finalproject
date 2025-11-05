using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUHD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    private int ammos = 0;
    
    private void Awake()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (ammoText != null)
            ammoText.text = ammos.ToString();
    }

    void Start()
    {
		ammos = AmmoManager.instance.currentAmmo;
        UpdateHUD();
    }

    void Update()
    {
        if (ScoreManager.instance != null)
        {
			ammos = AmmoManager.instance.currentAmmo;
            UpdateHUD();
        }
    }
}
