using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;

    private bool isPaused = false;

    void Start()
    {
        // Cacher le menu au démarrage
        pauseMenuUI.SetActive(false);

        // Initialiser le slider avec le volume actuel
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // S'assurer que le Canvas utilise "Unscaled Time"
        Canvas canvas = pauseMenuUI.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // Mettre au-dessus de tout
        }
    }

    void Update()
    {
        // Vérifier si Échap est pressé
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Arrêter le jeu
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Reprendre le jeu
        isPaused = false;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Réactiver le temps
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Réactiver le temps
        Application.Quit();
        Debug.Log("Quit Game");
    }
}