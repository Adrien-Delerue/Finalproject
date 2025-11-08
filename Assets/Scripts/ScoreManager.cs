using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] public int score = 0;
    private float timer = 0f;

    //delegate for score change event
    public delegate void ScoreChangedDelegate(int newScore);
    public ScoreChangedDelegate OnScoreChanged;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int points)
    {
        score += points;

        if (OnScoreChanged != null)
            OnScoreChanged(score);
    }

    void Update()
    {
        // Score increment over time
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            AddScore(1); 
            timer = 0f;
        }
    }
}