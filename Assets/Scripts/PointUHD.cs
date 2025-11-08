using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.ShaderData;

public class PointUHD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointText;
    private int points = 0;

    void Start()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.OnScoreChanged += UpdateScore;

            UpdateScore(ScoreManager.instance.score);
        }
    }

    void OnDestroy()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.OnScoreChanged -= UpdateScore;
        }
    }

    private void UpdateScore(int newScore)
    {
        points = newScore;

        if (pointText != null)
            pointText.text = points.ToString();
    }
}
