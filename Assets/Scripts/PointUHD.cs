using TMPro;
using UnityEngine;


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
