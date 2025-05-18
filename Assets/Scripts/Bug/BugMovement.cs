using System.Collections;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float rotationSmooth = 0.1f;

    [Header("Wander Settings")]
    [SerializeField] private float minChangeDirectionTime = 1f;
    [SerializeField] private float maxChangeDirectionTime = 3f;
    [SerializeField] private float minWaitTime = 0.5f;
    [SerializeField] private float maxWaitTime = 2f;

    [Header("Food Detection")]
    [SerializeField] private float foodDetectionRadius = 5f;
    [SerializeField] private LayerMask foodLayer;

    [Header("Obstacle Detection")]
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Coroutine wanderCoroutine;
    private Transform player;
    private Transform targetFood;
    private Bug bugScript;
    private BoxCollider2D bugCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bugScript = GetComponent<Bug>();
        bugCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        StartCoroutine(AlwaysScanForFood());
        StartWandering();
    }

    private IEnumerator AlwaysScanForFood()
    {
        var wait = new WaitForSeconds(0.2f);
        while (true)
        {
            FindClosestFood();
            yield return wait;
        }
    }

    private void FixedUpdate()
    {
        if (bugScript.isHostile && player != null)
        {
            ChasePlayer();
        }
        else if (bugScript.CanEat() && targetFood != null)
        {
            MoveTowardsFood();
        }
        else
        {
            Move();
        }
    }

    public void ClearFoodTarget()
    {
        targetFood = null;
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
            StartWandering();
        }
    }

    private void Move()
    {
        Vector2 frontPos = (Vector2)transform.position + moveDirection * 0.5f;
        float angle = transform.eulerAngles.z;
        if (Physics2D.OverlapBox(frontPos, bugCollider.size, angle, wallLayer))
            moveDirection = GetRandomDirection();

        Vector2 newPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        RotateTowards(moveDirection);
        rb.MovePosition(newPos);
    }

    private void MoveTowardsFood()
    {
        Vector2 dir = ((Vector2)targetFood.position - rb.position).normalized;
        RotateTowards(dir);
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }

    private void ChasePlayer()
    {
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        RotateTowards(dir);
        rb.MovePosition(rb.position + dir * chaseSpeed * Time.fixedDeltaTime);
    }

    private void RotateTowards(Vector2 direction)
    {
        if (direction == Vector2.zero) return;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float smoothed = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSmooth);
        rb.MoveRotation(smoothed);
    }

    private void StartWandering()
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
        float a = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    private void FindClosestFood()
    {
        Collider2D[] hits;

        if (bugScript.eatsOtherBugs)
        {
            hits = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius);
        }
        else
        {
            hits = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius, foodLayer);
        }

        Transform closest = null;
        float bestDist = float.PositiveInfinity;

        foreach (var c in hits)
        {
            if (c.gameObject == this.gameObject) continue; // Skip self

            bool isValidFood = !string.IsNullOrEmpty(bugScript.allowedFoodTag) && c.CompareTag(bugScript.allowedFoodTag);
            bool isValidBug = bugScript.eatsOtherBugs && !string.IsNullOrEmpty(bugScript.allowedBugTag) && c.CompareTag(bugScript.allowedBugTag);

            if (isValidFood || isValidBug)
            {
                float d = Vector2.Distance(transform.position, c.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    closest = c.transform;
                }
            }
        }

        targetFood = closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, foodDetectionRadius);

        if (bugCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector2.zero, bugCollider.size);
        }
    }
}
