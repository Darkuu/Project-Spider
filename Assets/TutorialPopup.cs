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
        public string message; // The tutorial message to display
        public Sprite icon;    // Optional icon for the tutorial
        public string actionToComplete; // The action that dismisses this step
    }

    public List<TutorialStep> tutorialSteps; // List of all tutorial steps
    public GameObject popupUI;  // The UI Panel for the tutorial
    public TMP_Text messageText;    // UI Text to display the message
    public Image iconImage;     // UI Image for the icon (optional)

    private int currentStep = 0;
    private bool isTutorialActive = false;

    private void Start()
    {
        popupUI.SetActive(false); // Hide tutorial on start
    }

    public void ShowTutorialStep(int stepIndex)
    {
        if (stepIndex >= tutorialSteps.Count) return;

        TutorialStep step = tutorialSteps[stepIndex];
        popupUI.SetActive(true);
        messageText.text = step.message;

        if (step.icon != null)
            iconImage.sprite = step.icon;
        else
            iconImage.gameObject.SetActive(false);

        isTutorialActive = true;
        currentStep = stepIndex;
    }

    public void CompleteStep(string action)
    {
        if (!isTutorialActive) return;

        if (tutorialSteps[currentStep].actionToComplete == action)
        {
            popupUI.SetActive(false);
            isTutorialActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTutorialActive)
        {
            ShowTutorialStep(0); // Show the first tutorial step when entering the area
        }
    }
}
