using UnityEngine;

public class EggItem : MonoBehaviour
{
    public ItemScript eggItem;
    public string eggType;
    public GameObject bugPrefab;
    // This script is only for holding the food's information, no need for triggers here

    [HideInInspector] public bool isCollected = false;

}
