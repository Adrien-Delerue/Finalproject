using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;

    private bool isPaused = false;

    void Start()
    {
        // Hide the pause menu at start
        pauseMenuUI.SetActive(false);

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

            //decrease volume
            if (Input.GetKey(KeyCode.LeftArrow))
                change = -0.1f * Time.unscaledDeltaTime;

            //increase volume
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

        // Select the slider so it’s ready for input
        if (volumeSlider != null)
            EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
    }

    public void Resume()
    {
        // Hide pause menu and resume time
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
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
        // Quit the game
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
