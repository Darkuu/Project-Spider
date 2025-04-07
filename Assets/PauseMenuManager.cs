using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuUI;    // Main pause menu panel

    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;          // Slider for background music volume
    [SerializeField] private Slider sfxSlider;            // Slider for SFX volume

    [Header("Audio Settings")]
    [SerializeField] private AudioClip pauseSound;        // Sound to play when pausing
    [SerializeField] private AudioClip resumeSound;       // Sound to play when resuming

    private bool isPaused = false;

    private void Start()
    {
        // Ensure the pause and options panels are off at the start
        pauseMenuUI.SetActive(false);

        // Initialize sliders with current volume values if AudioManager is available
        if (AudioManager.instance != null)
        {
            musicSlider.value = AudioManager.instance.musicVolume;
            sfxSlider.value = AudioManager.instance.sfxVolume;
        }
    }

    private void Update()
    {
        // Toggle pause/resume with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    /// <summary>
    /// Pauses the game: displays the pause menu, freezes time, and plays the pause sound.
    /// </summary>
    public void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time

        if (AudioManager.instance != null && pauseSound != null)
            AudioManager.instance.PlaySFX(pauseSound);
    }

    /// <summary>
    /// Resumes the game: hides pause and options panels, unfreezes time, and plays the resume sound.
    /// </summary>
    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time

        if (AudioManager.instance != null && resumeSound != null)
            AudioManager.instance.PlaySFX(resumeSound);
    }



    /// <summary>
    /// Called when the music slider's value changes.
    /// </summary>
    /// <param name="value">The new volume value (0-1).</param>
    public void OnMusicSliderChanged(float value)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetMusicVolume(value);
    }

    /// <summary>
    /// Called when the SFX slider's value changes.
    /// </summary>
    /// <param name="value">The new volume value (0-1).</param>
    public void OnSFXSliderChanged(float value)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetSFXVolume(value);
    }

    /// <summary>
    /// Called by a Quit button in the UI to exit the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
