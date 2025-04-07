using UnityEngine;

public class AudioManagerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject audioManagerPrefab;
    [SerializeField] private AudioClip spawnInSound;

    private void Start()
    {
        if (AudioManager.instance == null)
        {
            if (audioManagerPrefab != null)
            {
                GameObject obj = Instantiate(audioManagerPrefab);

                DontDestroyOnLoad(obj);
                AudioManager.instance.PlaySFX(spawnInSound);
            }
            else
            {
                Debug.LogError("AudioManager prefab is not assigned!");
            }
        }
    }
}
