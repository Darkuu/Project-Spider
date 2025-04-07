using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuButtons : MonoBehaviour
{
    public Animator transition;
    [SerializeField] private float transitionTime;
    [SerializeField] private float fadeOutDuration = 1f; 

    public void PlayGame()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        if (AudioManager.instance != null)
        {
            StartCoroutine(FadeOutMusic(fadeOutDuration));
        }

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(fadeOutDuration);
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Gameplay");
    }

    // Method to fade out music volume gradually
    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = AudioManager.instance.musicVolume; 
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            AudioManager.instance.SetMusicVolume(Mathf.Lerp(startVolume, 0f, timeElapsed / duration)); 
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        AudioManager.instance.SetMusicVolume(0f); 
    }
}
