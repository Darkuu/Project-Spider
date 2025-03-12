using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float playerMaxHealth;

    [SerializeField] private float currentHealth;

    private float iFramesDuration = 2f;  
    private float iFramesTimer = 0f;     

    private bool isInvincible = false; 

    void Start()
    {
        currentHealth = playerMaxHealth;
    }

    void Update()
    {
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
