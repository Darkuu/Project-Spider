using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

/// <summary> Sets a background color for this game object in the Unity Hierarchy window </summary>
[UnityEditor.InitializeOnLoad]
#endif
public class CustomHierarchyColor : MonoBehaviour
{
    private static Vector2 offset = new Vector2(20, 1);

    static CustomHierarchyColor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            Color backgroundColor = Color.white;
            Color textColor = Color.white;
            Texture2D texture = null;

            // Define conditions to color based on the name containing certain keywords
            string objName = obj.name.ToLower();  // Convert name to lowercase for case-insensitive comparison

            if (objName.Contains("background"))
            {
                backgroundColor = new Color(0.2f, 0.6f, 0.1f); // Light green for background
                textColor = new Color(0.9f, 0.9f, 0.9f); // White text
            }
            else if (objName.Contains("canvas"))
            {
                backgroundColor = new Color(0.7f, 0.45f, 0.0f); // Orange for canvas
                textColor = new Color(0.9f, 0.9f, 0.9f); // White text
            }
            else if (objName.Contains("ui"))
            {
                backgroundColor = new Color(0.4f, 0.2f, 0.8f); // Purple for UI elements
                textColor = new Color(0.9f, 0.9f, 0.9f); // White text
            }
            else if (objName.Contains("character"))
            {
                backgroundColor = new Color(0.01176f, 0.6745f, 0.7882f); // Blue for UI elements
                textColor = new Color(0.9f, 0.9f, 0.9f); // White text
            }
            else if (objName.Contains("[ph]"))
            {
                backgroundColor = new Color(1f, 0f, 0f); // Blue for UI elements
                textColor = new Color(0.9f, 0.9f, 0.9f); // White text
            }

            // If we have a color, draw it
            if (backgroundColor != Color.white)
            {
                Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
                Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

                EditorGUI.DrawRect(bgRect, backgroundColor);
                EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = textColor },
                    fontStyle = FontStyle.Bold
                });

                if (texture != null)
                    EditorGUI.DrawPreviewTexture(new Rect(selectionRect.position, new Vector2(selectionRect.height, selectionRect.height)), texture);
            }
        }
    }
}
