using UnityEngine;
using System.Collections;

public class NewspaperTerminal : MonoBehaviour
{
    [Header("UI")]
    public GameObject newspaperUI;
    public KeyCode openKey = KeyCode.E;
    public KeyCode closeKey = KeyCode.Escape;

    [Header("Prompt")]
    public GameObject interactionPrompt;
    private float promptAnimationDuration = 0.2f;
    private float bobbingSpeed = 5f;
    private float bobbingAmount = 0.1f;
    private Coroutine bobbingCoroutine;

    [Header("Quest Progression")]
    public string questID = "DefaultQuest";
    public int progressToStage = 1;
    public bool progressQuestOnOpen = true;

    private bool playerInRange = false;

    void Start()
    {
        if (newspaperUI != null)
            newspaperUI.SetActive(false);

        if (interactionPrompt != null)
            interactionPrompt.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(openKey))
        {
            ToggleNewspaper();
        }

        if (newspaperUI.activeSelf && Input.GetKeyDown(closeKey))
        {
            CloseNewspaper();
        }
    }

    void ToggleNewspaper()
    {
        bool isActive = newspaperUI.activeSelf;
        newspaperUI.SetActive(!isActive);

        if (!isActive)
        {
            // Opened
            if (progressQuestOnOpen && !string.IsNullOrEmpty(questID))
            {
                QuestManager.Instance.ProgressQuest(questID, progressToStage);
            }
        }
    }

    void CloseNewspaper()
    {
        if (newspaperUI != null)
            newspaperUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactionPrompt != null)
            {
                StartCoroutine(AnimatePrompt(interactionPrompt.transform, Vector3.one));
                bobbingCoroutine = StartCoroutine(BobPrompt(interactionPrompt.transform));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CloseNewspaper();

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
        float elapsed = 0f;
        Vector3 startScale = promptTransform.localScale;

        while (elapsed < promptAnimationDuration)
        {
            promptTransform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / promptAnimationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        promptTransform.localScale = targetScale;
    }

    private IEnumerator BobPrompt(Transform promptTransform)
    {
        Vector3 startPos = promptTransform.localPosition;

        while (true)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            promptTransform.localPosition = new Vector3(startPos.x, newY, startPos.z);
            yield return null;
        }
    }
}
