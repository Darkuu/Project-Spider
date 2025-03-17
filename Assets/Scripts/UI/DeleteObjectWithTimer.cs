using UnityEngine;

public class DeleteObjectWithTimer : MonoBehaviour
{
    public float destroyTime = 30f; 
    private bool isTouchingFarm = false;

    private void Start()
    {
        Invoke(nameof(CheckAndDestroy), destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Farm"))
        {
            isTouchingFarm = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Farm"))
        {
            isTouchingFarm = false;
        }
    }

    private void CheckAndDestroy()
    {
        if (!isTouchingFarm)
        {
            Destroy(gameObject);
        }
    }
}
