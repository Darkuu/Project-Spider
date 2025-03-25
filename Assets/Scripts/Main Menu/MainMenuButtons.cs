using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{

    public void LoadScene()
    {
        SceneManager.LoadScene("Gameplay");
    }

}
