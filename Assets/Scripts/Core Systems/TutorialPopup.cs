using UnityEngine;
using UnityEngine.UI;
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
    }

    public List<TutorialStep> tutorialSteps; 
    public GameObject popupUI;  
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image iconImage; 

    private int currentStep = -1;
    private bool isTutorialActive = false;

    private void Start()
    {
        popupUI.SetActive(false); 

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

        if (step.triggerZone != null)
            step.triggerZone.SetActive(false);

        popupUI.SetActive(true);
        titleText.text = step.title;
        descriptionText.text = step.description;

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
                    ShowTutorialStep(nextStep); 
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
            ShowTutorialStep(nextStep); 
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
