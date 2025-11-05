using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverScreen GameOverScreen;
    private bool isGameOver = false;

    public void GameOver()
    {
        if (!isGameOver) 
        {
            isGameOver = true;
            Time.timeScale = 0f; 
            GameOverScreen.Setup(ScoreManager.instance.score);
        }
    }

    void Update()
    {
        if (!isGameOver) 
        {
            if (HealthBar.instance.slider.value <= 0)
            {
                GameOver();
            }
        }
    }
}