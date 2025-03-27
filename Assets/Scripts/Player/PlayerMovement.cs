using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    [SerializeField] private float baseSpeed = 8f; 
    [SerializeField] private float sprintMultiplier = 1.5f; 

    [SerializeField] private Rigidbody2D rb;

    private TutorialPopup tutorialPopup;

    private void Start()
    {
       {
            tutorialPopup = FindObjectOfType<TutorialPopup>(); 
        }
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        
        float speed = GetCurrentSpeed();
        if ((horizontal != 0f || vertical != 0f))
        {
            if (tutorialPopup != null)
            {
                tutorialPopup.CompleteStep("playerMove");
            }
        }

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

}
