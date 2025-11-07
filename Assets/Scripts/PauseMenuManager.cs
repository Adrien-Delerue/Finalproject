using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject crossHair; // Ajout de la référence au CrossHair

    public bool isPaused = false;

    void Start()
    {
        // Hide the pause menu at start
        pauseMenuUI.SetActive(false);

        // S'assurer que le CrossHair est visible au démarrage
        if (crossHair != null)
            crossHair.SetActive(true);

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Get the Canvas and set it to be always on top
        Canvas canvas = pauseMenuUI.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = short.MaxValue; // highest priority

            // Make sure the Canvas has a GraphicRaycaster
            GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster == null)
                canvas.gameObject.AddComponent<GraphicRaycaster>();
        }

        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }

    void Update()
    {
        // Press Escape to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }

        if (isPaused && volumeSlider != null)
        {
            float change = 0f;

            // Decrease volume
            if (Input.GetKey(KeyCode.LeftArrow))
                change = -0.1f * Time.unscaledDeltaTime;

            // Increase volume
            if (Input.GetKey(KeyCode.RightArrow))
                change = 0.1f * Time.unscaledDeltaTime;

            if (change != 0f)
            {
                volumeSlider.value = Mathf.Clamp01(volumeSlider.value + change);
                SetVolume(volumeSlider.value);
            }
        }
    }

    public void Pause()
    {
        // Show pause menu and stop time
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Cacher le CrossHair
        if (crossHair != null)
            crossHair.SetActive(false);

        // Select the slider so it's ready for input
        if (volumeSlider != null)
            EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (crossHair != null)
            crossHair.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Quit Game");
    }
}