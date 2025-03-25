using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuButtons : MonoBehaviour
{
    public Animator transition;
    [SerializeField] private float transitionTime;

    public void PlayGame()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Gameplay");
    }
}
