using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip MainMenuMusicClip;
    void Start()
    {
        AudioManager.instance.PlayMusic(MainMenuMusicClip);
    }

    void Update()
    {
        
    }
}
