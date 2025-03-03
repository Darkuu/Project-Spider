using System.Collections;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float changeDirectionTime;
    [SerializeField] private float waitTime;
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
            yield return new WaitForSeconds(waitTime); 

            moveDirection = GetRandomDirection(); 
            yield return new WaitForSeconds(changeDirectionTime);
        }
    }

    private Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)).normalized;
    }
}
