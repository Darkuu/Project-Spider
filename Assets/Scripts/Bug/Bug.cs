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

    [Header("Hunger Settings")]
    [SerializeField] private float totalHunger = 1f;
    [SerializeField] private float hungerDecreaseRate = 0.01f;

    [Header("Special Behavior")]
    public bool isHostile;
    public bool eatsOtherBugs = false;

    [Tooltip("Tag that this bug can eat if it eats other bugs")]
    public string allowedBugTag;


    public float CurrentHunger { get; private set; }
    public bool IsHungry => CurrentHunger <= totalHunger * 0.5f;

    private float cooldownTimer;

    private void Start()
    {
        CurrentHunger = totalHunger * 0.5f;
        cooldownTimer = cooldownTime;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        CurrentHunger = Mathf.Clamp(
            CurrentHunger - hungerDecreaseRate * Time.deltaTime,
            0f,
            totalHunger
        );
    }

    public bool CanEat()
    {
        // Only non‑hostile bugs that are hungry and out of cooldown can chase/eat
        return !isHostile && IsHungry && cooldownTimer <= 0f;
    }

    public void FillHunger()
    {
        CurrentHunger = totalHunger;
        cooldownTimer = cooldownTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(allowedFoodTag) && cooldownTimer <= 0f)
        {
            Instantiate(poopPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            FillHunger();

            // Tell the movement script to clear its target
            GetComponent<BugMovement>().ClearFoodTarget();
        }
        else if (collision.gameObject.CompareTag("Player") && isHostile)
        {
            var stats = collision.gameObject.GetComponent<PlayerStats>();
            stats?.TakeDamage(damage, transform.position);
        }
        else if (eatsOtherBugs && collision.gameObject.CompareTag(allowedBugTag) && cooldownTimer <= 0f)
        {
            var otherBug = collision.gameObject.GetComponent<Bug>();
            if (otherBug != null && !otherBug.isHostile)
            {
                Destroy(collision.gameObject); 
                Instantiate(poopPrefab, transform.position, Quaternion.identity);
                FillHunger();
                GetComponent<BugMovement>().ClearFoodTarget();
            }
        }

    }
}
