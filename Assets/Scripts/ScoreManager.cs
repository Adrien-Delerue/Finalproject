using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] public int score = 0;

    private float timer = 0f;

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
       
    }
    void Update()
    {
		// Score increment over time
		timer += Time.deltaTime;
        if (timer >= 1f)
        {
            score += 1;
            timer = 0f;
        }
    }
}