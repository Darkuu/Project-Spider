using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    private float baseSpeed = 8f; // Default speed
    private float sprintMultiplier = 1.5f; // Sprint speed multiplier
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Flip(); // Flip character if needed
    }

    private void FixedUpdate()
    {
        float speed = GetCurrentSpeed();
        // Keep natural diagonal movement speed
        Vector2 moveDirection = new Vector2(horizontal, vertical);
        // Prevent excessive speed diagonally
        if (moveDirection.magnitude > 1)
        {
            moveDirection = moveDirection.normalized;
        }

        rb.linearVelocity = moveDirection * speed; 
    }


    private float GetCurrentSpeed()
    {
        // Check sprint unlock from PlayerUnlockManager
        if (PlayerUnlockManager.instance.IsAbilityUnlocked(PlayerUnlockManager.Unlockable.Sprint)
            && Input.GetKey(KeyCode.LeftShift))
        {
            return baseSpeed * sprintMultiplier;
        }
        return baseSpeed;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
