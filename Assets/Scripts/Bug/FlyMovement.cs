using System.Collections;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float changeDirectionTime = 2f;
    [SerializeField] private float waitTime = 2f; // Time before moving again
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
            moveDirection = Vector2.zero; // Stop moving
            yield return new WaitForSeconds(waitTime); // Wait before moving again

            moveDirection = GetRandomDirection(); // Pick a new direction
            yield return new WaitForSeconds(changeDirectionTime); // Move for a while
        }
    }

    private Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)).normalized;
    }
}