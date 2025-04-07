using System.Collections;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float foodDetectionRadius = 5f;
    [SerializeField] private string foodTag = "Food";

    [Header("Random Time Settings")]
    [SerializeField] private float minChangeDirectionTime = 1f;
    [SerializeField] private float maxChangeDirectionTime = 3f;
    [SerializeField] private float minWaitTime = 0.5f;
    [SerializeField] private float maxWaitTime = 2f;

    [Header("Hunger Settings")]
    [SerializeField] private float hungerThreshold = 0.5f; // Value from 0 to 1, where 1 means completely hungry
    [SerializeField] private float currentHunger = 1f; // Initially hungry

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Transform targetFood;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
    }

    private void FixedUpdate()
    {
        if (currentHunger >= hungerThreshold && targetFood != null)
        {
            MoveTowardsFood();
        }
        else
        {
            Wander();
        }
    }

    private void Wander()
    {
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void MoveTowardsFood()
    {
        if (targetFood == null) return;

        Vector2 directionToFood = ((Vector2)targetFood.position - rb.position).normalized;
        Vector2 newPosition = rb.position + directionToFood * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            moveDirection = Vector2.zero;
            float waitDuration = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitDuration);

            moveDirection = GetRandomDirection();
            float moveDuration = Random.Range(minChangeDirectionTime, maxChangeDirectionTime);
            yield return new WaitForSeconds(moveDuration);
        }
    }

    private Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)).normalized;
    }

    private void Update()
    {
        FindClosestFood();
    }

    private void FindClosestFood()
    {
        Collider2D[] nearbyFoods = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius);

        Transform closestFood = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D foodCollider in nearbyFoods)
        {
            if (foodCollider.CompareTag(foodTag))
            {
                float distance = Vector2.Distance(transform.position, foodCollider.transform.position);

                if (distance < closestDistance)
                {
                    closestFood = foodCollider.transform;
                    closestDistance = distance;
                }
            }
        }

        targetFood = closestFood;
    }

    public void EatFood()
    {
        currentHunger = 0f; // Reset hunger after eating
    }

    public void IncreaseHunger(float amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0f, 1f);
    }

    // Getter and Setter for currentHunger
    public float CurrentHunger
    {
        get { return currentHunger; }
        set { currentHunger = Mathf.Clamp(value, 0f, 1f); } // Ensures the hunger level stays within 0 and 1
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, foodDetectionRadius);
    }
}
