using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [Tooltip("How many seconds before this object is destroyed")]
    public float despawnTime;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }
}
