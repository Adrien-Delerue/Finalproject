using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);

    }

    public void RestartButton()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButton()
    {
        Time.timeScale = 1f; 
        Application.Quit();
        Debug.Log("Quit Game");
    }
}