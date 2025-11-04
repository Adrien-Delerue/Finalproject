using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverScreen GameOverScreen;

    public void GameOver()
    {
        GameOverScreen.Setup(100);
    }

    void Update()
    {

        if (HealthBar.instance.slider.value <= 0)
        {
            GameOver();
        }

        // Si le joueur appuie sur Échap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOver();
        }
    }
}
