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
        if (stepIndex >= tutorialSteps.Count) return;

        currentStep = stepIndex;
        TutorialStep step = tutorialSteps[stepIndex];

        if (step.triggerZone != null)
            step.triggerZone.SetActive(false);

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
    }

    public void CompleteStep(string action)
    {
        if (!isTutorialActive || currentStep == -1) return;

        if (tutorialSteps[currentStep].actionToComplete == action)
        {
            tutorialStepCompleteSound.Play();
            popupUI.SetActive(false);
            isTutorialActive = false;

            int nextStep = currentStep + 1;
            if (nextStep < tutorialSteps.Count)
            {
                if (tutorialSteps[nextStep].autoTriggerNextStep)
                {
                    Invoke(nameof(TriggerNextStep), stepDelay); 
                }
            }
        }
    }

    public void TriggerNextStep()
    {
        int nextStep = currentStep + 1;
        if (nextStep < tutorialSteps.Count)
        {
            ShowTutorialStep(nextStep);
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
