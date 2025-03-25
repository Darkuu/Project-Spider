using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
 public Image image;
 public Color selectedColour, notSelectedColour;
    private Vector3 originalScale;
    private Vector3 selectedScale;


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
    }

    public void Deselect()
    {
        image.color = notSelectedColour;
        transform.localScale = originalScale; 
    }

    public void OnDrop(PointerEventData eventData)

 {
  if(transform.childCount == 0)

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
