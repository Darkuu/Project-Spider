using System.Collections;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.5f;

    [Header("Wander Settings")]
    [SerializeField] private float minChangeDirectionTime = 1f;
    [SerializeField] private float maxChangeDirectionTime = 3f;
    [SerializeField] private float minWaitTime = 0.5f;
    [SerializeField] private float maxWaitTime = 2f;

    [Header("Hunger and Food")]
    [SerializeField] private float foodDetectionRadius = 5f;
    [SerializeField] private float hungerThreshold = 0.5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Coroutine wanderCoroutine;
    private Transform player;
    private Transform targetFood;

    public float CurrentHunger { get; private set; } = 1f;

    private Bug bugScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bugScript = GetComponent<Bug>();
        Wander();
    }

    private void FixedUpdate()
    {
        if (bugScript.isHostile && player != null)
        {
            ChasePlayer();
        }
        else if (CurrentHunger >= hungerThreshold && targetFood != null)
        {
            MoveTowardsFood();
        }
        else
        {
            Move();
        }
    }

    private void Update()
    {
        DecreaseHunger();

        if (!bugScript.isHostile)
        {
            FindClosestFood();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && bugScript.isHostile)
        {
            player = other.transform;
            StopWandering();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && bugScript.isHostile)
        {
            player = null;
            Wander();
        }
    }

    private void Move()
    {
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void MoveTowardsFood()
    {
        if (targetFood == null) return;

        Vector2 direction = ((Vector2)targetFood.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * chaseSpeed * Time.fixedDeltaTime);
    }

    private void Wander()
    {
        if (wanderCoroutine != null) StopCoroutine(wanderCoroutine);
        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    private void StopWandering()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }
        moveDirection = Vector2.zero;
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            moveDirection = Vector2.zero;
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            moveDirection = GetRandomDirection();
            yield return new WaitForSeconds(Random.Range(minChangeDirectionTime, maxChangeDirectionTime));
        }
    }

    private Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void FindClosestFood()
    {
        Collider2D[] nearbyFoods = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius);

        Transform closest = null;
        float shortest = Mathf.Infinity;

        foreach (Collider2D food in nearbyFoods)
        {
            if (food.CompareTag(bugScript.allowedFoodTag))
            {
                float distance = Vector2.Distance(transform.position, food.transform.position);
                if (distance < shortest)
                {
                    closest = food.transform;
                    shortest = distance;
                }
            }
        }

        targetFood = closest;
    }

    private void DecreaseHunger()
    {
        CurrentHunger = Mathf.Clamp01(CurrentHunger - Time.deltaTime * 0.01f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, foodDetectionRadius);
    }
    public void FillHunger()
    {
        CurrentHunger = 1f;
    }

}
