using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PointUHD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointText;
    private int points = 0;

    private void Awake()
    {
        UpdateHUD();
    }

    public int Points
    {
        get => points;
        set
        {
            points = value;
            UpdateHUD();
        }
    }

    private void UpdateHUD()
    {
        if (pointText != null)
            pointText.text = points.ToString();
    }

    void Start()
    {
        Points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ajouter 1 point quand la touche Espace est enfoncée
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Points++;
        }
    }
}