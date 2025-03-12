using UnityEngine;
using TMPro; // Import TextMeshPro

public class PlayerStats : MonoBehaviour
{
    public float playerMaxHealth = 100f;
    [SerializeField] private float currentHealth;

    private float iFramesDuration = 2f;
    private float iFramesTimer = 0f;
    private bool isInvincible = false;

    [Header("UI Elements")]
    public TMP_Text healthText; // Assign in Inspector

    void Start()
    {
        currentHealth = playerMaxHealth;
        UpdateHealthUI();
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
        UpdateHealthUI(); // Update UI when health changes

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

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, playerMaxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{playerMaxHealth}";
        }
    }
}
