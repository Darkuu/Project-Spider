using UnityEngine;

public class GizmoDrawer : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public Color gizmoColor = Color.green;
    public float gizmoSize = 0.3f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }
}
