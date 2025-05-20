using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SellBox : MonoBehaviour
{
    public AudioClip sellSound;

    [Header("Pitch Settings")]
    [SerializeField] private float basePitch = 1.0f;
    [SerializeField] private float pitchMultiplier = 1.2f;
    [SerializeField] private float maxPitch = 2.5f;
    [SerializeField] private float resetTime = 5f;

    [Header("Selling Limits")]
    [SerializeField] private int maxSalesPerEggType = 5;
    [SerializeField] private float cooldownDuration = 120f;

    [Header("Floating Text")]
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform textSpawnPoint;

    private Coroutine resetCoroutine;
    private float previousPitch = 1f;
    private TutorialPopup tutorialPopup;

    private class EggSaleData
    {
        public int salesCount = 0;
        public float cooldownEndTime = 0f;
    }

    private Dictionary<string, EggSaleData> salesTracker = new Dictionary<string, EggSaleData>();

    private void Start()
    {
        tutorialPopup = FindFirstObjectByType<TutorialPopup>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EggItem egg = other.GetComponent<EggItem>();
        if (egg != null && egg.eggItem != null)
        {
            string eggType = egg.eggItem.name;

            if (!salesTracker.ContainsKey(eggType))
                salesTracker[eggType] = new EggSaleData();

            EggSaleData saleData = salesTracker[eggType];

            if (Time.time < saleData.cooldownEndTime)
            {
                float waitTime = saleData.cooldownEndTime - Time.time;
                ShowFloatingText($"Hole is full of this egg type!\n Please wait!", Color.red);
                return; 
            }

            if (saleData.salesCount >= maxSalesPerEggType)
            {
                saleData.cooldownEndTime = Time.time + cooldownDuration;
                saleData.salesCount = 0;
                ShowFloatingText($"Hole is full of this egg type!\n Please wait!", Color.red);
                return; 
            }

            // SUCCESSFUL SALE
            saleData.salesCount++;
            tutorialPopup?.CompleteStep("SellEgg");

            float value = egg.eggItem.sellValue;
            MoneyManager.instance.AddCoins(value);
            ShowFloatingText($"+{value}   <sprite name=Coin>" , Color.green);

            PlaySellSound();

            Destroy(other.gameObject); 
        }
    }


    private void PlaySellSound()
    {
        previousPitch = Mathf.Min(previousPitch * pitchMultiplier, maxPitch);
        AudioManager.instance.PlaySFX(sellSound, 1f, previousPitch);

        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);

        resetCoroutine = StartCoroutine(ResetPitchAfterDelay());
    }

    private IEnumerator ResetPitchAfterDelay()
    {
        yield return new WaitForSeconds(resetTime);
        previousPitch = basePitch;
    }

    private void ShowFloatingText(string message, Color color)
    {
        if (floatingTextPrefab == null || textSpawnPoint == null) return;

        GameObject obj = Instantiate(floatingTextPrefab, textSpawnPoint.position, Quaternion.identity);
        FloatingText floating = obj.GetComponent<FloatingText>();
        if (floating != null)
            floating.SetText(message, color);
    }
}
