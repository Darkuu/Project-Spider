using UnityEngine;
using TMPro; // Import TextMeshPro
using UnityEngine.UI; // Import UI elements
using Unity.Cinemachine; // Import Cinemachine
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public float playerMaxHealth = 100f;
    [SerializeField] private float currentHealth;

    private float iFramesDuration = 2f;
    private bool isInvincible = false;

    private float regenCooldown = 20f;
    private float regenRate = 5f; // Health per second
    private Coroutine regenCoroutine;

    [Header("UI Elements")]
    public TMP_Text healthText;
    public GameObject deathUI; // Assign this in the Inspector

    [Header("Respawn Settings")]
    public Transform respawnPoint; // Assign this in the Inspector
    private float respawnDelay = 5f;

    [Header("Camera Settings")]
    public CinemachineCamera playerCamera; // Assign the correct Cinemachine virtual camera in the Inspector

    private GameObject[] allCameras;

    void Start()
    {
        currentHealth = playerMaxHealth;
        UpdateHealthUI();
        allCameras = GameObject.FindGameObjectsWithTag("Camera"); // Cache cameras for performance
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth > 0)
        {
            StartIFrames();
            if (regenCoroutine != null) StopCoroutine(regenCoroutine); // Stop regen if taking damage
            regenCoroutine = StartCoroutine(StartHealthRegen());
        }
        else
        {
            Die();
        }
    }

    private void StartIFrames()
    {
        isInvincible = true;
        Invoke(nameof(EndIFrames), iFramesDuration);
    }

    private void EndIFrames()
    {
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Player has died!!!!");
        deathUI.SetActive(true); // Show death UI
        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position; // Move player to respawn point
        currentHealth = playerMaxHealth; // Restore health
        UpdateHealthUI();
        deathUI.SetActive(false); // Hide death UI
        SwitchToPlayerCamera();
        if (regenCoroutine != null) StopCoroutine(regenCoroutine); // Reset regen on respawn
        regenCoroutine = StartCoroutine(StartHealthRegen());
    }

    private void SwitchToPlayerCamera()
    {
        foreach (GameObject cam in allCameras)
        {
            cam.SetActive(false);
        }
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            playerCamera.Priority = 10; // Ensure player's camera has the highest priority
        }
    }

    public void Heal(float healAmount)
    {
        if (currentHealth < playerMaxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, playerMaxHealth);
            UpdateHealthUI();
        }
    }

    private IEnumerator StartHealthRegen()
    {
        yield return new WaitForSeconds(regenCooldown);

        while (currentHealth < playerMaxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + regenRate * Time.deltaTime, playerMaxHealth);
            UpdateHealthUI();
            yield return null;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(playerMaxHealth)}";
        }
    }
}
