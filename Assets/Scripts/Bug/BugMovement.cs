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

    [Header("Food Detection")]
    [SerializeField] private float foodDetectionRadius = 5f;

    [Header("Layer Mask Settings")]
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Coroutine wanderCoroutine;
    private Transform player;
    private Transform targetFood;

    private Bug bugScript;

    private BoxCollider2D bugCollider;  // The bug's Collider2D (we'll use this for detection)

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bugScript = GetComponent<Bug>();
        bugCollider = GetComponent<BoxCollider2D>(); // Get the bug's BoxCollider2D
        Wander();

        if (!bugScript.isHostile)
        {
            StartCoroutine(CheckForFoodRoutine());
        }
    }

    private IEnumerator CheckForFoodRoutine()
    {
        while (true)
        {
            if (bugScript.IsHungry)
            {
                FindClosestFood();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void FixedUpdate()
    {
        if (bugScript.isHostile && player != null)
        {
            ChasePlayer();
        }
        else if (bugScript.IsHungry && targetFood != null)
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

    private float directionChangeCooldown = 0.5f;
    private float timeSinceLastDirectionChange = 0f;

    private void Move()
    {
        timeSinceLastDirectionChange += Time.fixedDeltaTime;

        // Move the bug forward based on its current direction
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // Check if the bug's collider touches anything in front (based on the direction it's moving)
        if (IsObstacleInFront())
        {
            moveDirection = GetRandomDirection(); // Change direction if there's an obstacle
            timeSinceLastDirectionChange = 0f;
        }

        // Move the bug and rotate it
        RotateTowards(moveDirection);
        rb.MovePosition(newPosition);
    }

    private bool IsObstacleInFront()
    {
        // Check if the bug's collider is touching an obstacle in front (using its current direction)
        Vector2 frontPosition = (Vector2)transform.position + moveDirection * 0.5f;  // Move a little forward
        Collider2D hit = Physics2D.OverlapBox(frontPosition, bugCollider.size, 0f, wallLayer);

        return hit != null;
    }

    private void MoveTowardsFood()
    {
        if (targetFood == null) return;

        Vector2 direction = ((Vector2)targetFood.position - rb.position).normalized;
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        RotateTowards(targetFood.position);
        rb.MovePosition(newPosition);
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        Vector2 newPosition = rb.position + direction * chaseSpeed * Time.fixedDeltaTime;
        RotateTowards(player.position);
        rb.MovePosition(newPosition);
    }

    private void RotateTowards(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f;
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, 0.1f);
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
        float angle = Random.Range(0f, 360f);
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, foodDetectionRadius);

        // Draw a box around the bug to visualize its collision area
        if (bugCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, bugCollider.size);
        }
    }
}