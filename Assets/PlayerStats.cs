using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float playerMaxHealth;

    [SerializeField] private float currentHealth;

    private float iFramesDuration = 1f;  // Duration of invincibility frames (in seconds)
    private float iFramesTimer = 0f;     // Timer to track iFrame duration

    private bool isInvincible = false;   // Flag to track if the player is invincible

    void Start()
    {
        currentHealth = playerMaxHealth;
    }

    void Update()
    {
        // Decrease iFrames timer over time, making player invincible for a limited time
        if (isInvincible)
        {
            iFramesTimer -= Time.deltaTime;

            if (iFramesTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth > 0)
        {
            StartIFrames();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void StartIFrames()
    {
        isInvincible = true;
        iFramesTimer = iFramesDuration;
    }

    private void Die()
    {
        Debug.Log("Player has died!!!!");
    }
}
