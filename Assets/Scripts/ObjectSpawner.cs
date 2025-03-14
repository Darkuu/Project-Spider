using UnityEngine;
using System.Collections;
public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; 
    public float spawnCooldown;      
    private GameObject currentObject; 

    void Start()
    {
        StartCoroutine(SpawnObjectRoutine());
    }

    private IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            if (currentObject == null)
            {
                SpawnObject();
            }
            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    // Function to spawn a new object
    private void SpawnObject()
    {
        currentObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}
