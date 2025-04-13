using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtonClicking : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;
    public float alphaThreshold = 0.1f;

    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshold;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover detected!");
        if (hoverSound != null && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(hoverSound);
        }
    }
}
