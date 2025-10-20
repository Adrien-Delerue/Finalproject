using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class PointUHD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointText;
    int points = 0;

    private void Awake()
    {
        UpdateHUD();
    }
    public int Points
    {
        get { return points; }
        set
        {
            points = value;
            UpdateHUD();
        }
    }
    private void UpdateHUD()
    {
        pointText.text =  points.ToString();
    }
        
     void Start()
    {
        points = 0;
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        //add point if space button pressed
        if (Input.GetKeyUp(KeyCode.Escape)) {
            points += 1;
            UpdateHUD();
        }
    }
    
}
