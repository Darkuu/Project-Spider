using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    public GameObject objectToDelete;
    public ShopTerminal spawnedFromTerminal; // Reference to the hidden terminal

    public void DeleteObjects()
    {
        if (objectToDelete != null)
        {
            Destroy(objectToDelete);

            if (spawnedFromTerminal != null)
            {
                Debug.Log("Restoring terminal: " + spawnedFromTerminal.name);
                spawnedFromTerminal.gameObject.SetActive(true); // Show the terminal again
            }
            else
            {
                Debug.LogWarning("No terminal assigned! Cannot restore.");
            }
        }
    }
}
