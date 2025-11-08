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
    [SerializeField] private GameObject crossHair;

    public AudioSource mobOnFlagMusic;
    public AudioSource backgroundMusic;
    private bool mobOnFlagPlaying = false;

    public bool isPaused = false;

    void Awake()
    {
		// Singleton pattern to ensure only one instance exists
		if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

		// Volume initialization
		AudioListener.volume = 0.5f;
	}
	void Start()
    {
        // Hide the pause menu at start
        pauseMenuUI.SetActive(false);

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Select the slider so it's ready for input
        if (volumeSlider != null)
            EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);

        //Pausing mobOnFlagMusic if playing
        if (mobOnFlagMusic.isPlaying)
        {
            mobOnFlagPlaying = true;
            mobOnFlagMusic.Pause();
            backgroundMusic.UnPause();
        }
    }

    public void Resume()
    {
        // Hide pause menu and resume time
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (crossHair != null)
            crossHair.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mobOnFlagPlaying)
        {
            mobOnFlagPlaying = false;
            mobOnFlagMusic.UnPause();
            backgroundMusic.Pause();
        }

    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Application.Quit();
        Debug.Log("Quit Game");
    }
}