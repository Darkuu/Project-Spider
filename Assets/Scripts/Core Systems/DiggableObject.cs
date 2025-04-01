using UnityEngine;
using TMPro;
using System.Collections;

public class DiggableObjects : MonoBehaviour
{
    public GameObject objectToSpawn;
    public KeyCode digKey = KeyCode.E;
    public Sprite dugSprite; // The new sprite to indicate the spot is dug
    private bool hasBeenDug = false;
    private bool playerInRange = false;
    private SpriteRenderer spriteRenderer;

    [Header("UI Elements")]
    [Tooltip("Object that appears when in range")]
    public GameObject interactionPrompt;
    private float promptAnimationDuration = 0.2f;
    private float bobbingSpeed = 5f;
    private float bobbingAmount = 0.1f;
    private Coroutine bobbingCoroutine;

    [Header("Tutorial Popup")]
    public TutorialPopup tutorialPopup;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (interactionPrompt != null)
            interactionPrompt.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (!hasBeenDug && playerInRange && Input.GetKeyDown(digKey))
        {
            DigObject();
        }
    }

    private void DigObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }

        if (dugSprite != null)
        {
            spriteRenderer.sprite = dugSprite; 
        }

        hasBeenDug = true; 
        interactionPrompt.SetActive(false);

        if (tutorialPopup != null)
        {
            tutorialPopup.ShowTutorialStep(4); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenDug)
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
            if (!gameObject.activeInHierarchy) yield break;
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
            if (!gameObject.activeInHierarchy) yield break;
            float newY = startPos.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            promptTransform.localPosition = new Vector3(startPos.x, newY, startPos.z);
            yield return null;
        }
    }
}
