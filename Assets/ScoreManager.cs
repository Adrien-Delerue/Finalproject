using UnityEngine;
using UnityEngine.UI; // pour l'affichage du score

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // permet d'y accéder depuis d'autres scripts

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
        // Incrément du score avec le temps
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            score += 1;
            timer = 0f;
        }
    }
}