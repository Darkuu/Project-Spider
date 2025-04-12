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
        public string title;
        public string description;
        public Sprite icon;
        public string actionToComplete;
        public GameObject triggerZone;
        public bool autoTriggerNextStep;

        public bool autoCompleteStep = false;
        public float autoCompleteDelay = 3f;
    }

    public List<TutorialStep> tutorialSteps;
    public GameObject popupUI;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image iconImage;
    public AudioClip notificationSound;
    public AudioClip tutorialStepCompleteSound;

    public float stepDelay = 2f;
    public KeyCode closeKey = KeyCode.X;

    private int currentStep = -1;
    private bool isTutorialActive = false;
    private HashSet<string> completedActions = new HashSet<string>();

    private void Start()
    {
        popupUI.SetActive(false); // Ensure popup is hidden at the start

        // Set up trigger zones if they exist
        for (int i = 0; i < tutorialSteps.Count; i++)
        {
            var step = tutorialSteps[i];
            if (step.triggerZone != null)
            {
                step.triggerZone.SetActive(true);
                step.triggerZone.AddComponent<TutorialStepTrigger>().Setup(this, i);
            }
        }

        // Start tutorial if there are steps
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

        // Hide the trigger zone after it has been triggered
        if (step.triggerZone != null)
        {
            step.triggerZone.SetActive(false);
        }

        // Show the tutorial UI and set the content
        popupUI.SetActive(true);
        titleText.text = step.title;
        descriptionText.text = step.description;
        AudioManager.instance.PlaySFX(notificationSound);

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

        // Auto-complete step if enabled
        if (step.autoCompleteStep)
        {
            StartCoroutine(AutoCompleteStepAfterDelay(step.actionToComplete, step.autoCompleteDelay));
        }
    }

    public void CompleteStep(string action)
    {
        if (completedActions.Contains(action)) return;

        completedActions.Add(action);

        // Mark current step as complete and hide the UI
        if (isTutorialActive && currentStep != -1 && tutorialSteps[currentStep].actionToComplete == action)
        {
            AudioManager.instance.PlaySFX(tutorialStepCompleteSound);
            popupUI.SetActive(false); // Hide the tutorial UI when completed
            isTutorialActive = false;

            // Move to next step if available
            int nextStep = currentStep + 1;
            if (nextStep < tutorialSteps.Count)
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

    public void CloseTutorial()
    {
        // Just hide the UI, but don't stop the tutorial flow
        popupUI.SetActive(false);
        isTutorialActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(closeKey)) // Close the tutorial UI when the player presses the close key
        {
            CloseTutorial();
        }
    }
}

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
