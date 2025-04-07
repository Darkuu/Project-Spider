using Unity.Cinemachine;
using UnityEngine;

public class CameraZoneSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner2D confiner;

    public void SetCameraBounds(Collider2D newBounds)
    {
        confiner.BoundingShape2D = newBounds;

        // Trick to force update (CinemachineConfiner2D doesn't expose cache methods anymore)
        confiner.enabled = false;
        confiner.enabled = true;
    }
}
