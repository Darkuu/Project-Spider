using UnityEngine;

public class Bug : MonoBehaviour
{
    [Header("Bug Stats Interaction")]
    public ItemScript bugItem;

    [Header("Food Interaction")]
    public string allowedFoodTag;
    public GameObject poopPrefab;

    [Header("Bug Brain Logic")]
    public int damage;

    [Header("Hunger Settings")]
    public float cooldownTime = 30f;
    [SerializeField] private float totalHunger = 1f;
    [SerializeField] private float hungerDecreaseRate = 0.01f;
    private float currentHunger;  


    [Header("Special Behavior")]
    public bool isHostile;
    public bool eatsOtherBugs = false;

    [Tooltip("Tag that this bug can eat if it eats other bugs")]
    public string allowedBugTag;

    [SerializeField, Tooltip("Current hunger value (read-only)")]

    public float CurrentHunger
    {
        get => currentHunger;
        private set => currentHunger = value;
    }

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
        return IsHungry && cooldownTimer <= 0f;
    }

    public void FillHunger()
    {
        CurrentHunger = totalHunger;
        cooldownTimer = cooldownTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!string.IsNullOrEmpty(allowedFoodTag) &&
            collision.gameObject.CompareTag(allowedFoodTag) &&
            cooldownTimer <= 0f)
        {
            if (poopPrefab != null)
                Instantiate(poopPrefab, transform.position, Quaternion.identity);

            Destroy(collision.gameObject);
            FillHunger();

            var movement = GetComponent<BugMovement>();
            if (movement != null)
                movement.ClearFoodTarget();
        }
        else if (collision.gameObject.CompareTag("Player") && isHostile)
        {
            var stats = collision.gameObject.GetComponent<PlayerStats>();
            stats?.TakeDamage(damage, transform.position);
        }
        else if (eatsOtherBugs &&
                 !string.IsNullOrEmpty(allowedBugTag) &&
                 collision.gameObject.CompareTag(allowedBugTag) &&
                 cooldownTimer <= 0f)
        {
            var otherBug = collision.gameObject.GetComponent<Bug>();
            if (otherBug != null && !otherBug.isHostile)
            {
                Destroy(collision.gameObject);

                if (poopPrefab != null)
                    Instantiate(poopPrefab, transform.position, Quaternion.identity);

                FillHunger();

                var movement = GetComponent<BugMovement>();
                if (movement != null)
                    movement.ClearFoodTarget();
            }
        }
    }
}
