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
    [SerializeField] private float hungerThreshold = 0.5f;

    public float CurrentHunger { get; private set; }
    public bool IsHungry => CurrentHunger <= hungerThreshold * totalHunger;


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
}
