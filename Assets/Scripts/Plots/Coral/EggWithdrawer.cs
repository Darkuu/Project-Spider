using UnityEngine;
using System.Collections;

public class EggWithdrawer : MonoBehaviour
{
    [Header("Collector Reference")]
    public EggCollector eggCollector;  // Assign this in the Inspector

    private bool isPlayerInRange = false;

    [Header("UI Elements")]
    [Tooltip("Object that appears when in range")]
    public GameObject interactionPrompt;
    private float promptAnimationDuration = 0.2f;
    private float bobbingSpeed = 5f;
    private float bobbingAmount = 0.1f;

    private Coroutine bobbingCoroutine;
    private Coroutine animationCoroutine;

    private void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.transform.localScale = Vector3.zero;

        if (eggCollector == null)
            Debug.LogWarning("EggCollector not assigned in EggWithdrawer.");
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            try
            {
                // Unity null check: handles destroyed objects
                if (eggCollector != null && eggCollector.gameObject != null)
                {
                    eggCollector.DropAllEggs();
                    Debug.Log("Eggs withdrawn from collector.");
                }
                else
                {
                    Debug.LogError("EggCollector has been destroyed or is not assigned.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error withdrawing eggs: " + ex.Message);
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
                // Stop existing animation before starting a new one
                if (animationCoroutine != null)
                    StopCoroutine(animationCoroutine);

                animationCoroutine = StartCoroutine(AnimatePrompt(interactionPrompt.transform, Vector3.one));

                if (bobbingCoroutine == null)
                    bobbingCoroutine = StartCoroutine(BobPrompt(interactionPrompt.transform));
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
                if (animationCoroutine != null)
                    StopCoroutine(animationCoroutine);

                animationCoroutine = StartCoroutine(AnimatePrompt(interactionPrompt.transform, Vector3.zero));
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
        if (promptTransform == null)
            yield break;

        float elapsedTime = 0f;
        Vector3 initialScale = promptTransform.localScale;

        while (elapsedTime < promptAnimationDuration)
        {
            if (!gameObject.activeInHierarchy || promptTransform == null)
                yield break;

            promptTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / promptAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        promptTransform.localScale = targetScale;
    }

    private IEnumerator BobPrompt(Transform promptTransform)
    {
        if (promptTransform == null)
            yield break;

        Vector3 startPos = promptTransform.localPosition;

        while (true)
        {
            if (!gameObject.activeInHierarchy || promptTransform == null)
                yield break;

            float newY = startPos.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            promptTransform.localPosition = new Vector3(startPos.x, newY, startPos.z);
            yield return null;
        }
    }
}
