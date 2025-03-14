using UnityEngine;

public class SellBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has an EggItem component.
        EggItem egg = other.GetComponent<EggItem>();
        if (egg != null && egg.eggItem != null)
        {
            // Get the sell value from the egg's ItemScript.
            int value = egg.eggItem.sellValue;
            // Add coins to the player's coin count.
            MoneyManager.instance.AddCoins(value);
            Debug.Log("Egg sold for " + value + " coins!");

            // Remove the egg from the scene.
            Destroy(other.gameObject);
        }
    }
}