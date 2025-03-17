using UnityEngine;
using TMPro;
using System.Collections;

public class ShopTerminal : MonoBehaviour
{
    public GameObject shopUI;
    public KeyCode openKey = KeyCode.E;
    public KeyCode closeKey = KeyCode.Escape;
    private bool playerInRange = false;

    [Header("Spawn Settings")]
    [Tooltip("Where it spawns objects, if it spawns any")]
    public Transform spawnPoint;

    [Header("UI Elements")]
    [Tooltip("Object that appears when in range")]
    public GameObject interactionPrompt;
    private float promptAnimationDuration = 0.2f;
    private float bobbingSpeed = 5f;
    private float bobbingAmount = 0.1f;

    private Coroutine bobbingCoroutine;

    private void Start()
    {
        if (shopUI != null)
            shopUI.SetActive(false);

        if (interactionPrompt != null)
            interactionPrompt.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(openKey))
        {
            ToggleShop();
        }

        if (shopUI.activeSelf && Input.GetKeyDown(closeKey))
        {
            CloseTerminal();
        }
    }

    void ToggleShop()
    {
        if (shopUI != null)
        {
            bool isActive = shopUI.activeSelf;
            shopUI.SetActive(!isActive);

            if (shopUI.activeSelf)
            {
                // UI is now OPEN
                UIManager.instance.OpenUI();

                ShopUIController controller = shopUI.GetComponent<ShopUIController>();
                if (controller != null)
                {
                    controller.currentTerminal = this;
                }
            }
            else
            {
                // UI is now CLOSED
                UIManager.instance.CloseUI();
            }
        }
    }

    public void CloseAndRemoveTerminal()
    {
        CloseTerminal();
        Destroy(gameObject);
    }

    public void CloseTerminal()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
            UIManager.instance.CloseUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionPrompt != null)
            {
                StartCoroutine(AnimatePrompt(interactionPrompt.transform, Vector3.one));
                if (bobbingCoroutine == null)
                {
                    bobbingCoroutine = StartCoroutine(BobPrompt(interactionPrompt.transform));
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CloseTerminal();

            // Only start animation if the object is active
            if (gameObject.activeInHierarchy && interactionPrompt != null)
            {
                StartCoroutine(AnimatePrompt(interactionPrompt.transform, Vector3.zero));
            }

            if (bobbingCoroutine != null)
            {
                StopCoroutine(bobbingCoroutine);
                bobbingCoroutine = null;
            }
        }
    }

    private IEnumerator AnimatePrompt(Transform promptTransform, Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = promptTransform.localScale;

        while (elapsedTime < promptAnimationDuration)
        {
            if (!gameObject.activeInHierarchy) yield break; // Stop coroutine if object is inactive
            promptTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / promptAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        promptTransform.localScale = targetScale;
    }

    private IEnumerator BobPrompt(Transform promptTransform)
    {
        Vector3 startPos = promptTransform.localPosition;
        while (true)
        {
            if (!gameObject.activeInHierarchy) yield break; // Stop coroutine if object is inactive
            float newY = startPos.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            promptTransform.localPosition = new Vector3(startPos.x, newY, startPos.z);
            yield return null;
        }
    }
}
