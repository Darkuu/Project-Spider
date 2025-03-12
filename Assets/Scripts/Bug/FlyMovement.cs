using System.Collections;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [Header("Random Time Settings")]
    [SerializeField] private float minChangeDirectionTime = 1f;
    [SerializeField] private float maxChangeDirectionTime = 3f;
    [SerializeField] private float minWaitTime = 0.5f;
    [SerializeField] private float maxWaitTime = 2f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
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
}
