using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    [SerializeField] private AudioClip inventorySelectSound;
    public Color selectedColour, notSelectedColour;
    private Vector3 originalScale, selectedScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        selectedScale = originalScale * 1.1f;
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColour;
        transform.localScale = selectedScale;
        // Add a null-check for AudioManager and the audio clip
        if (AudioManager.instance != null && inventorySelectSound != null)
        {
            AudioManager.instance.PlaySFX(inventorySelectSound);
        }
        else
        {
            Debug.LogWarning("AudioManager.instance or inventorySelectSound is null in InventorySlot.Select()");
        }
    }

    public void Deselect()
    {
        image.color = notSelectedColour;
        transform.localScale = originalScale;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
            draggableItem.parentAfterDrag = transform;
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
            GameObject current = transform.GetChild(0).gameObject;
            InventoryItem currentDraggable = current.GetComponent<InventoryItem>();
            currentDraggable.transform.SetParent(draggableItem.parentAfterDrag);
            draggableItem.parentAfterDrag = transform;
        }
    }
}
