using UnityEngine;

public class Bug : MonoBehaviour
{
    [Header("Bug Stats Interaction")]
    public ItemScript bugItem;

    [Header("Food Interaction")]
    public string allowedFoodTag;
    public GameObject poopPrefab;

    [Header("Bug Brain Logic")]
    public float cooldownTime = 30f;
    public int damage;
    public bool isHostile;

    private float cooldownTimer;
    private BugMovement bugMovement;

    private void Start()
    {
        cooldownTimer = cooldownTime;
        bugMovement = GetComponent<BugMovement>();
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(allowedFoodTag) && cooldownTimer <= 0f)
        {
            DropPoop();
            Destroy(other.gameObject);
            cooldownTimer = cooldownTime;

            // Refill hunger after eating
            bugMovement.FillHunger();
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

    private void DropPoop()
    {
        Instantiate(poopPrefab, transform.position, Quaternion.identity);
    }
}
