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

    [Header("Hunger Settings")]
    [SerializeField] private float totalHunger = 1f;
    [SerializeField] private float hungerDecreaseRate = 0.01f;

    public float CurrentHunger { get; private set; }

    private bool isHungry = false;  // Manage hunger state here
    public bool IsHungry => isHungry || CurrentHunger <= 0f;  // Hungry when either isHungry is true or hunger is <= 0

    private float cooldownTimer;
    private BugMovement bugMovement;

    private void Start()
    {
        cooldownTimer = cooldownTime;
        bugMovement = GetComponent<BugMovement>();
        CurrentHunger = totalHunger;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        DecreaseHunger();
    }

    private void DecreaseHunger()
    {
        CurrentHunger = Mathf.Clamp(CurrentHunger - hungerDecreaseRate * Time.deltaTime, 0f, totalHunger);
    }

    public void FillHunger()
    {
        CurrentHunger = totalHunger;
        isHungry = false;  // Reset hunger state when filled
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(allowedFoodTag) && cooldownTimer <= 0f)
        {
            DropPoop();
            Destroy(other.gameObject);
            cooldownTimer = cooldownTime;
            FillHunger();
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

    // Triggered while the bug is touching something
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(allowedFoodTag))
        {
            // If the bug is touching food and its hunger is 0 or lower, make it hungry
            if (CurrentHunger <= 0f)
            {
                isHungry = true;  // Set the hungry state to true
            }
        }
    }
}
