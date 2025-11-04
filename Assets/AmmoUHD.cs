using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUHD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    private int Ammo = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        UpdateHUD();
    }
    private void UpdateHUD()
    {
        if (ammoText != null)
            ammoText.text = Ammo.ToString();
    }

    void Start()
    {
        Ammo = AmmoManager.instance.currentAmmo;
        UpdateHUD();

    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.instance != null)
        {
            Ammo = AmmoManager.instance.currentAmmo;
            UpdateHUD();
        }

    }
}
