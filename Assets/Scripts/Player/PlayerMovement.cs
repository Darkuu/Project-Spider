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

    private void Start()
    {
        tutorialPopup = FindFirstObjectByType<TutorialPopup>();
    }

    void Update()
    {
        if (isKnockedBack) return;

        
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (isKnockedBack) return;

        float speed = GetCurrentSpeed();
        Vector2 moveDirection = new Vector2(horizontal, vertical);
        if (moveDirection.magnitude > 1)
        {
            moveDirection = moveDirection.normalized;
        }
        rb.linearVelocity = moveDirection * speed;
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
        float knockbackDuration = 0.5f;  

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


    private IEnumerator ResetMovementAfterKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false; 
    }
}
