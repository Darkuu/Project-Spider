using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class Teleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportLocation;
    public CinemachineCamera newAreaCamera;
    public PolygonCollider2D newCameraBounds;
    private bool isTeleporting = false;
    private bool isInTeleporterArea = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTeleporting && !isInTeleporterArea)
        {
            isInTeleporterArea = true;
            StartCoroutine(TeleportSequence(other.transform));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTeleporterArea = false;
        }
    }

    private IEnumerator TeleportSequence(Transform player)
    {
        isTeleporting = true;
        player.position = teleportLocation.position;
        SwitchToNewCamera();
        yield return StartCoroutine(ScreenFader.instance.FadeToBlack());
        yield return StartCoroutine(ScreenFader.instance.FadeFromBlack());
        isTeleporting = false;
    }

    private void SwitchToNewCamera()
    {
        CinemachineCamera currentCamera = Object.FindFirstObjectByType<CinemachineCamera>();
        if (currentCamera != null)
        {
            currentCamera.gameObject.SetActive(false);
        }

        if (newAreaCamera != null)
        {
            newAreaCamera.gameObject.SetActive(true);
            CinemachineConfiner2D confiner = newAreaCamera.GetComponent<CinemachineConfiner2D>();
            if (confiner != null && newCameraBounds != null)
            {
                confiner.BoundingShape2D = newCameraBounds;
                confiner.InvalidateBoundingShapeCache();
            }
        }
    }
}
