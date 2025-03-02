using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class Teleporter : MonoBehaviour
{
    [Header("Teleport Destination Colliders")]
    public Collider2D firstAreaCollider;   // Collider defining the first area's destination
    public Collider2D secondAreaCollider;  // Collider defining the second area's destination

    [Header("Camera Settings")]
    public CinemachineCamera firstCamera;     // Camera for the first area
    public PolygonCollider2D firstCameraBounds;  // Bounds for confining the first camera
    public CinemachineCamera secondCamera;    // Camera for the second area
    public PolygonCollider2D secondCameraBounds; // Bounds for confining the second camera

    private bool isInFirstArea = true; // Tracks if the player is currently in the first area
    private bool isTeleporting = false;
    private bool isInTeleporterArea = false; // Prevents multiple triggers

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

        // Teleport the player to the center of the destination collider
        if (isInFirstArea)
        {
            if (secondAreaCollider != null)
            {
                player.position = secondAreaCollider.bounds.center;
            }
            SwitchToSecondCamera();
        }
        else
        {
            if (firstAreaCollider != null)
            {
                player.position = firstAreaCollider.bounds.center;
            }
            SwitchToFirstCamera();
        }

        // Toggle current area flag
        isInFirstArea = !isInFirstArea;

        // Fade from black after teleporting
        yield return StartCoroutine(ScreenFader.instance.FadeFromBlack());

        isTeleporting = false;
    }

    private void SwitchToFirstCamera()
    {
        if (secondCamera != null)
            secondCamera.gameObject.SetActive(false);

        if (firstCamera != null)
        {
            firstCamera.gameObject.SetActive(true);
            CinemachineConfiner2D confiner = firstCamera.GetComponent<CinemachineConfiner2D>();
            if (confiner != null && firstCameraBounds != null)
            {
                confiner.BoundingShape2D = firstCameraBounds;
                confiner.InvalidateBoundingShapeCache();
            }
        }
    }

    private void SwitchToSecondCamera()
    {
        if (firstCamera != null)
            firstCamera.gameObject.SetActive(false);

        if (secondCamera != null)
        {
            secondCamera.gameObject.SetActive(true);
            CinemachineConfiner2D confiner = secondCamera.GetComponent<CinemachineConfiner2D>();
            if (confiner != null && secondCameraBounds != null)
            {
                confiner.BoundingShape2D = secondCameraBounds;
                confiner.InvalidateBoundingShapeCache();
            }
        }
    }
}
