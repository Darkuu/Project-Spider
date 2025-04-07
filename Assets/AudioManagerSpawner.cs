using UnityEngine;

public class AudioManagerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject audioManagerPrefab; 

    private void Start()
    {
        if (AudioManager.instance == null)
        {
            if (audioManagerPrefab != null)
            {
                GameObject obj = Instantiate(audioManagerPrefab);
                DontDestroyOnLoad(obj);
            }
            else
            {
                Debug.LogError("AudioManager prefab is not assigned!");
            }
        }
    }
}
