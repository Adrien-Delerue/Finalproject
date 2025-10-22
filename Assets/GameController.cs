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
        // Si le joueur appuie sur �chap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOver();
        }
    }
}
