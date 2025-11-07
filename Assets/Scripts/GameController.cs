using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance; // Ajout pour singleton
    public GameOverScreen GameOverScreen;
    [SerializeField] private GameObject crossHair;
    private bool isGameOver = false;

    void Awake()
    {
        // Singleton pour éviter les duplicatas
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
        // Réinitialiser l'état au démarrage de la scène
        isGameOver = false;
        Time.timeScale = 1f;

        // S'assurer que le crosshair est visible
        if (crossHair != null)
            crossHair.SetActive(true);

        // S'assurer que le GameOver screen est caché
        if (GameOverScreen != null)
            GameOverScreen.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Time.timeScale = 0f;

            // Cacher le CrossHair
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