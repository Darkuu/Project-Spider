using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class Teleporter : MonoBehaviour
{
    [Header("Teleport Destination Colliders")]
    public Collider2D firstAreaCollider;
    public Collider2D secondAreaCollider;

    [Header("Camera & Confiner")]
    public CinemachineCamera mainCamera;
    public CinemachineConfiner2D cameraConfiner;
    public PolygonCollider2D firstCameraBounds;
    public PolygonCollider2D secondCameraBounds;

    private bool isInFirstArea = true;
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

        // Fade to black before teleporting
        yield return StartCoroutine(ScreenFader.instance.FadeToBlack());

        if (isInFirstArea)
        {
            if (secondAreaCollider != null)
                player.position = secondAreaCollider.bounds.center;

            SetCameraBounds(secondCameraBounds);
        }
        else
        {
            if (firstAreaCollider != null)
                player.position = firstAreaCollider.bounds.center;

            SetCameraBounds(firstCameraBounds);
        }

        isInFirstArea = !isInFirstArea;

        // Fade from black after teleporting
        yield return StartCoroutine(ScreenFader.instance.FadeFromBlack());

        isTeleporting = false;
    }

    private void SetCameraBounds(PolygonCollider2D newBounds)
    {
        if (cameraConfiner != null && newBounds != null)
        {
            cameraConfiner.BoundingShape2D = newBounds;

            // Force refresh (Cinemachine 3.x)
            cameraConfiner.enabled = false;
            cameraConfiner.enabled = true;
            mainCamera.ForceCameraPosition(mainCamera.Follow.position, Quaternion.identity);
        }
    }
}
