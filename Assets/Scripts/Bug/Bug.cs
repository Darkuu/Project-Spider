using UnityEngine;

public class Bug : MonoBehaviour
{
    [Header("Bug Stats Interaction")]
    [Tooltip("Item the player receives upon pickup")]
    public ItemScript bugItem;

    [Header("Food Interaction")]
    [Tooltip("What kind of food can it eat")]
    public string allowedFoodTag;
    [Tooltip("What it poops out upon eating")]
    public GameObject poopPrefab;

    [Header("Bug Brain Logic")]
    [Tooltip("Cooldown between eating")]
    public float cooldownTime = 30f; 

    [Tooltip("What it poops out upon eating")]
    public int damage;

    public bool isHostile;

    private float cooldownTimer;
   

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(allowedFoodTag) && cooldownTimer <= 0f)
        {
            DropPoop();
            Destroy(other.gameObject);
            cooldownTimer = cooldownTime;
        }
        else if (other.gameObject.CompareTag("Player") && isHostile)
        {
            PlayerStats playerHealth = other.gameObject.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position); 
            }
        }

    }

    void DropPoop()
    {
        Instantiate(poopPrefab, transform.position, Quaternion.identity);
    }


    private void Start()
    {
        cooldownTimer = cooldownTime;
    }
    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
