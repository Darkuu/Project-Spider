using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TutorialPopup : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        [Header("Tutorial Step Properties")]
        public string title;
        public string description;
        public Sprite icon;
        public string actionToComplete;
        public GameObject triggerZone;
        public bool autoTriggerNextStep;

        [Header("Auto Complete")]
        public bool autoCompleteStep = false;
        public float autoCompleteDelay = 3f; // seconds
    }

    [Header("Tutorial Steps")]
    public List<TutorialStep> tutorialSteps;

    [Header("Assignables")]
    public GameObject popupUI;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image iconImage;
    public AudioSource notificationSound;
    public AudioSource tutorialStepCompleteSound;

    public float stepDelay = 2f;

    private int currentStep = -1;
    private bool isTutorialActive = false;

    private HashSet<string> completedActions = new HashSet<string>();

    private void Start()
    {
        popupUI.SetActive(false);

        for (int i = 0; i < tutorialSteps.Count; i++)
        {
            var step = tutorialSteps[i];
            if (step.triggerZone != null)
            {
                step.triggerZone.SetActive(true);
                step.triggerZone.AddComponent<TutorialStepTrigger>().Setup(this, i);
            }
        }

        if (tutorialSteps.Count > 0)
        {
            Invoke(nameof(StartTutorial), stepDelay);
        }
    }

    private void StartTutorial()
    {
        ShowTutorialStep(0);
    }

    public void ShowTutorialStep(int stepIndex)
    {
        // Skip already completed steps
        while (stepIndex < tutorialSteps.Count && completedActions.Contains(tutorialSteps[stepIndex].actionToComplete))
        {
            stepIndex++;
        }

        if (stepIndex >= tutorialSteps.Count) return;

        currentStep = stepIndex;
        var step = tutorialSteps[stepIndex];

        if (step.triggerZone != null)
        {
            step.triggerZone.SetActive(false);
        }

        popupUI.SetActive(true);
        titleText.text = step.title;
        descriptionText.text = step.description;
        notificationSound.Play();

        if (step.icon != null)
        {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = step.icon;
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }

        isTutorialActive = true;

        // ✅ NEW: Start auto-complete if enabled
        if (step.autoCompleteStep)
        {
            StartCoroutine(AutoCompleteStepAfterDelay(step.actionToComplete, step.autoCompleteDelay));
        }
    }

    public void CompleteStep(string action)
    {
        if (completedActions.Contains(action)) return;

        completedActions.Add(action);

        // If this is the current step, mark as complete and optionally move to next
        if (isTutorialActive && currentStep != -1 && tutorialSteps[currentStep].actionToComplete == action)
        {
            tutorialStepCompleteSound.Play();
            popupUI.SetActive(false);
            isTutorialActive = false;

            int nextStep = currentStep + 1;

            if (nextStep < tutorialSteps.Count && tutorialSteps[nextStep].autoTriggerNextStep)
            {
                Invoke(nameof(TriggerNextStep), stepDelay);
            }
        }
    }

    public void TriggerNextStep()
    {
        ShowTutorialStep(currentStep + 1);
    }

    private IEnumerator AutoCompleteStepAfterDelay(string action, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isTutorialActive && tutorialSteps[currentStep].actionToComplete == action)
        {
            CompleteStep(action);
        }
    }
}

// ✅ Still bundled for simplicity
class TutorialStepTrigger : MonoBehaviour
{
    private TutorialPopup tutorialPopup;
    private int stepIndex;

    public void Setup(TutorialPopup popup, int index)
    {
        tutorialPopup = popup;
        stepIndex = index;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialPopup.Invoke(nameof(DelayedStepTrigger), tutorialPopup.stepDelay);
        }
    }

    private void DelayedStepTrigger()
    {
        tutorialPopup.ShowTutorialStep(stepIndex);
    }
}
