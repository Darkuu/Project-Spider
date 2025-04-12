using UnityEngine;
using System.Collections;

public class EggWithdrawer : MonoBehaviour
{
    [Header("Collector Reference")]
    public EggCollector eggCollector;  // Assign the specific EggCollector from the Inspector

    private bool isPlayerInRange = false;

    [Header("UI Elements")]
    [Tooltip("Object that appears when in range")]
    public GameObject interactionPrompt;
    private float promptAnimationDuration = 0.2f;
    private float bobbingSpeed = 5f;
    private float bobbingAmount = 0.1f;

    private Coroutine bobbingCoroutine;
    private void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (eggCollector != null)
            {
                eggCollector.DropAllEggs(); // Call DropAllEggs instead of WithdrawEggs
                Debug.Log("Eggs withdrawn from collector.");
            }
            else
            {
                Debug.LogError("No EggCollector assigned to withdrawer.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered the withdrawer area.");

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
            isPlayerInRange = false;
            Debug.Log("Player left the withdrawer area.");

            if (interactionPrompt != null)
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