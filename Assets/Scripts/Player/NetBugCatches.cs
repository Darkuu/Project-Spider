﻿using UnityEngine;
using System.Collections;

public class NetBugCatcher : MonoBehaviour
{
    [Header("Net Settings")]
    [SerializeField] private Vector2 boxSize = new Vector2(3f, 3f);

    [Header("Capture Settings")]
    [SerializeField] private LayerMask captureLayer;
    [SerializeField] private Transform player;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip captureSound1;
    [SerializeField] private AudioClip captureSound2;

    [Header("Placement Settings")]
    [SerializeField] private float initialDropCooldown = 0.5f;
    [SerializeField] private float minDropCooldown = 0.1f;
    [SerializeField] private float dropAcceleration = 0.05f;

    [Header("Cooldown Settings")]
    [SerializeField] private float pickupToPlaceCooldown = 0.25f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem captureEffect;

    private float currentDropCooldown;
    private float lastCaptureTime = -Mathf.Infinity;
    private bool isPlacing;
    private TutorialPopup tutorialPopup;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        tutorialPopup = FindFirstObjectByType<TutorialPopup>();
        currentDropCooldown = initialDropCooldown;
    }

    private void Update()
    {
        if (UIManager.instance.isUIOpen) return;

        RotateAroundPlayer();

        if (Input.GetMouseButtonDown(1)) Capture();

        if (Input.GetMouseButtonDown(0) && !isPlacing)
        {
            StartCoroutine(PlaceItemRoutine());
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPlacing = false;
            currentDropCooldown = initialDropCooldown;
        }
    }

    private void Capture()
    {
        Vector2 center = (Vector2)transform.position;
        float rotation = transform.rotation.eulerAngles.z;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, boxSize, rotation, captureLayer);

        if (hitColliders.Length == 0) return;

        foreach (Collider2D collider in hitColliders)
        {
            if (!collider.isTrigger)
            {
                ItemScript item = TryCaptureItem(collider);
                if (item != null)
                {
                    Destroy(collider.gameObject);
                    lastCaptureTime = Time.time; // Start cooldown after capture

                    // Play sound and particle effect at the caught object's position
                    PlayRandomSound();
                    PlayCaptureEffect(collider.transform.position);
                }
            }
        }
    }

    private ItemScript TryCaptureItem(Collider2D collider)
    {
        ItemScript item = null;

        if (collider.TryGetComponent(out Bug bugComp) && bugComp.bugItem != null)
        {
            item = bugComp.bugItem;
            tutorialPopup?.CompleteStep("CatchBug");
        }
        else if (collider.TryGetComponent(out Food foodComp) && foodComp.foodItem != null)
        {
            item = foodComp.foodItem;
            tutorialPopup?.CompleteStep("CatchFood");
        }
        else if (collider.TryGetComponent(out EggItem eggComp) && eggComp.eggItem != null)
        {
            item = eggComp.eggItem;
            tutorialPopup?.CompleteStep("CatchEgg");
        }

        return (item != null && InventoryManager.instance.AddItem(item)) ? item : null;
    }

    private IEnumerator PlaceItemRoutine()
    {
        isPlacing = true;

        while (Input.GetMouseButton(0))
        {
            PlaceItem();
            yield return new WaitForSeconds(currentDropCooldown);

            currentDropCooldown = Mathf.Max(minDropCooldown, currentDropCooldown - dropAcceleration);
        }

        isPlacing = false;
        currentDropCooldown = initialDropCooldown;
    }

    private void PlaceItem()
    {
        if (Time.time - lastCaptureTime < pickupToPlaceCooldown)
            return;

        ItemScript selectedItem = InventoryManager.instance.GetSelectedItem(false);
        if (selectedItem != null)
        {
            Vector2 spawnPosition = (Vector2)transform.position;
            if (selectedItem.type == ItemType.Bug || selectedItem.type == ItemType.Food || selectedItem.type == ItemType.Egg)
            {
                PlayRandomSound();
                InventoryManager.instance.GetSelectedItem(true);
                selectedItem.Use(spawnPosition);
            }
        }
    }

    private void PlayRandomSound()
    {
        if (captureSound1 == null || captureSound2 == null) return;

        AudioClip soundToPlay = Random.Range(0f, 1f) > 0.5f ? captureSound1 : captureSound2;
        AudioManager.instance.PlaySFX(soundToPlay);
    }

    private void PlayCaptureEffect(Vector3 position)
    {
        if (captureEffect == null) return;

        ParticleSystem effect = Instantiate(captureEffect, position, Quaternion.identity);
        effect.Play();

        Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
    }

    private void RotateAroundPlayer()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos2D = (Vector2)player.position;

        Vector2 direction = (mousePosition - playerPos2D).normalized;
        transform.position = playerPos2D + direction * 1f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 center = (Vector2)transform.position;
        float rotation = transform.rotation.eulerAngles.z;

        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, rotation), Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, boxSize);
    }
}
