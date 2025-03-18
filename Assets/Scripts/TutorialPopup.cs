using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TutorialPopup : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string message; // The tutorial message to display
        public Sprite icon;    // Optional icon for the tutorial
        public string actionToComplete; // The action that dismisses this step
        public GameObject triggerZone; // The trigger that activates this step (optional)
        public bool autoTriggerNextStep; // If true, this step triggers after the last step
    }

    public List<TutorialStep> tutorialSteps; // List of tutorial steps
    public GameObject popupUI;  // The UI Panel for the tutorial
    public TMP_Text messageText; // UI Text to display the message
    public Image iconImage; // UI Image for the icon (optional)

    private int currentStep = -1;
    private bool isTutorialActive = false;

    private void Start()
    {
        popupUI.SetActive(false); // Hide tutorial on start

        // Enable all trigger zones at start and assign trigger detection
        foreach (var step in tutorialSteps)
        {
            if (step.triggerZone != null)
            {
                step.triggerZone.SetActive(true);
                step.triggerZone.AddComponent<TutorialStepTrigger>().Setup(this, tutorialSteps.IndexOf(step));
            }
        }
    }

    public void ShowTutorialStep(int stepIndex)
    {
        if (stepIndex >= tutorialSteps.Count) return;

        currentStep = stepIndex;
        TutorialStep step = tutorialSteps[stepIndex];

        // Disable the trigger for this step if it has one
        if (step.triggerZone != null)
            step.triggerZone.SetActive(false);

        popupUI.SetActive(true);
        messageText.text = step.message;

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
    }

    public void CompleteStep(string action)
    {
        if (!isTutorialActive || currentStep == -1) return;

        if (tutorialSteps[currentStep].actionToComplete == action)
        {
            popupUI.SetActive(false);
            isTutorialActive = false;

            int nextStep = currentStep + 1;
            if (nextStep < tutorialSteps.Count)
            {
                if (tutorialSteps[nextStep].autoTriggerNextStep)
                {
                    ShowTutorialStep(nextStep); // Automatically show the next step
                }
            }
        }
    }

    // This method is used when the tutorial is triggered by zone instead of completing a step
    public void TriggerNextStep()
    {
        int nextStep = currentStep + 1;
        if (nextStep < tutorialSteps.Count)
        {
            ShowTutorialStep(nextStep); // Automatically show the next step
        }
    }
}

// This class is automatically added to tutorial trigger zones
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
            tutorialPopup.ShowTutorialStep(stepIndex);
        }
    }
}
