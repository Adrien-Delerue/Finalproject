using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance; // Ajout pour singleton
    public GameOverScreen GameOverScreen;
    [SerializeField] private GameObject crossHair;
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isGameOver = false;
        Time.timeScale = 1f;

        if (crossHair != null)
            crossHair.SetActive(true);

        if (GameOverScreen != null)
            GameOverScreen.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Time.timeScale = 0f;

            if (crossHair != null)
                crossHair.SetActive(false);

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