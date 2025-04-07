using UnityEngine;

public class AudioManagerSpawner : MonoBehaviour
{
    [SerializeField] private AudioClip spawnInSound;

    private void Start()
    {

        AudioManager.instance.PlaySFX(spawnInSound);
    }
}
