using UnityEngine;

public class FeederSpeedUI : MonoBehaviour
{
    private Feeder feeder;

    private void Start()
    {
        feeder = GetComponentInParent<Feeder>();
    }

    public void SetSlowSpeed()
    {
        feeder.SetFeedingSpeed(1);
    }

    public void SetMediumSpeed()
    {
        feeder.SetFeedingSpeed(2);
    }

    public void SetFastSpeed()
    {
        feeder.SetFeedingSpeed(3);
    }
}