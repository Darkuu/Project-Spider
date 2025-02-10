using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemScript[] itemstoPickUp;


    public void PickUpItem(int id)
    {
        bool result = inventoryManager.AddItem(itemstoPickUp[id]);
        if (result == true)
        {
            Debug.Log("item addeD");
        }
        else
        {
            Debug.Log("item not addeD");
        }

    }
    
    public void GetSelectedItem()
       {
           ItemScript receivedItem = inventoryManager.GetSelectedItem(false);
           if (receivedItem != null)
           {
               Debug.Log(receivedItem);
           }
           else
           {
               Debug.Log("No item selected!");
           }
       }
    public void UseSelectedItem()
    {
        ItemScript receivedItem = inventoryManager.GetSelectedItem(true);
        if (receivedItem != null)
        {
            Debug.Log("Item used!");
        }
        else
        {
            Debug.Log("No item used!");
        }
    }
}