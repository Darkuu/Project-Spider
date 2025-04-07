using UnityEngine;
using TMPro; // Import TextMeshPro
using UnityEngine.UI; // Import UI elements
using Unity.Cinemachine; // Import Cinemachine
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public TMP_Text healthText;
    public float playerMaxHealth = 100f;
    [SerializeField] private float currentHealth;

    private float iFramesDuration = 2f;
    private bool isInvincible = false;

    private float regenCooldown = 20f;
    private float regenRate = 5f;
    private Coroutine regenCoroutine;

    [Header("Death Elements")]
    public GameObject deathUI;
    public AudioClip deathSound;

    [Header("Respawn Settings")]
    public Transform respawnPoint;
    private float respawnDelay = 5f;

    [Header("Camera Settings")]
    public CinemachineCamera playerCamera;

    [Header("Knockback Settings")]
    public PlayerMovement playerMovement; // Reference to PlayerMovement script

    private GameObject[] allCameras;

    void Start()
    {
        currentHealth = playerMaxHealth;
        UpdateHealthUI();
        allCameras = GameObject.FindGameObjectsWithTag("Camera");
    }

    public void TakeDamage(int damage, Vector2 damageSource)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        UpdateHealthUI();

        // Trigger knockback when taking damage
        if (playerMovement != null)
        {
            playerMovement.ApplyKnockback(damageSource); // Call ApplyKnockback from PlayerMovement
        }

        if (currentHealth > 0)
        {
            StartIFrames();
            if (regenCoroutine != null) StopCoroutine(regenCoroutine);
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
        deathUI.SetActive(true);
        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        AudioManager.instance.PlaySFX(deathSound);
        transform.position = respawnPoint.position;
        currentHealth = playerMaxHealth;
        UpdateHealthUI();
        deathUI.SetActive(false);
        SwitchToPlayerCamera();
        if (regenCoroutine != null) StopCoroutine(regenCoroutine);
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
            playerCamera.Priority = 10;
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
            healthText.text = $"{Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(playerMaxHealth)}";
        }
    }
}
