using System.Collections;
using UnityEngine;

public class IceBugMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float changeDirectionTime = 2f;
    [SerializeField] private float waitTime = 1f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Coroutine wanderCoroutine;
    private Transform player;
    private bool isChasing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Wander();
    }

    private void FixedUpdate()
    {
        if (isChasing && player != null)
        {
            ChasePlayer();
        }
        else
        {
            Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            isChasing = true;
            StopWandering();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false;
            Wander();
        }
    }

    private void Move()
    {
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void StopMoving()
    {
        moveDirection = Vector2.zero;
    }

    public void Wander()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
        }
        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    private void StopWandering()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }
        StopMoving();
    }

    private IEnumerator WanderRoutine()
    {
        while (!isChasing) 
        {
            StopMoving();
            yield return new WaitForSeconds(waitTime);

            ChooseRandomDirection();
            yield return new WaitForSeconds(changeDirectionTime);
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
    }

    private void ChooseRandomDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
