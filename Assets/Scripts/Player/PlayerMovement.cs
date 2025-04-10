using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    [SerializeField] private float baseSpeed = 8f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    [SerializeField] private Rigidbody2D rb;

    private TutorialPopup tutorialPopup;
    private bool isKnockedBack = false;
    private Vector2 knockbackDirection;
    private bool hasMoved = false;

    // Smooth movement variables
    private Vector2 currentVelocity;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 8f; 

    private void Start()
    {
        tutorialPopup = FindFirstObjectByType<TutorialPopup>();
    }

    void Update()
    {
        if (isKnockedBack) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Check for first movement input
        if (!hasMoved && (horizontal != 0 || vertical != 0))
        {
            hasMoved = true;
            tutorialPopup?.CompleteStep("playerMove");
        }
    }

    private void FixedUpdate()
    {
        if (isKnockedBack) return;

        float targetSpeed = GetCurrentSpeed();
        Vector2 moveDirection = new Vector2(horizontal, vertical);
        
        // Normalize the movement direction if it exceeds 1

        if (moveDirection.magnitude > 1)
        {
            moveDirection = moveDirection.normalized;
        }

        // Smooth acceleration and deceleration
        if (moveDirection.magnitude > 0)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, moveDirection * targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
        }
        rb.linearVelocity = currentVelocity;
    }

    private float GetCurrentSpeed()
    {
        if (PlayerUnlockManager.instance.IsAbilityUnlocked(PlayerUnlockManager.Unlockable.Sprint) &&
            Input.GetKey(KeyCode.LeftShift))
        {
            return baseSpeed * sprintMultiplier;
        }
        return baseSpeed;
    }

    public void ApplyKnockback(Vector2 damageSource)
    {
        if (rb == null) return;

        knockbackDirection = (transform.position - (Vector3)damageSource).normalized;
        rb.linearVelocity = knockbackDirection * knockbackForce;  
        isKnockedBack = true;
        StartCoroutine(GradualKnockbackReduction());
    }

    private IEnumerator GradualKnockbackReduction()
    {
        float initialForce = knockbackForce;
        float elapsedTime = 0f;

        while (elapsedTime < knockbackDuration)
        {
            float percentageComplete = elapsedTime / knockbackDuration;
            float currentKnockbackForce = Mathf.Lerp(initialForce, 0f, percentageComplete);
            rb.linearVelocity = knockbackDirection * currentKnockbackForce;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.linearVelocity = Vector2.zero;  
        isKnockedBack = false;
    }
}
