using UnityEngine;
using System.Collections;

public class SellBox : MonoBehaviour
{
    public AudioSource sellSound;

    [SerializeField] private float basePitch = 1.0f;
    [SerializeField] private float pitchMultiplier = 1.2f;
    [SerializeField] private float maxPitch = 2.5f; 
    [SerializeField] private float resetTime = 5f;
    private Coroutine resetCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EggItem egg = other.GetComponent<EggItem>();
        if (egg != null && egg.eggItem != null)
        {
            int value = egg.eggItem.sellValue;
            MoneyManager.instance.AddCoins(value);
            Debug.Log("Egg sold for " + value + " coins!");

            PlaySellSound();
            Destroy(other.gameObject);
        }
    }

    private void PlaySellSound()
    {
        if (sellSound != null)
        {
            sellSound.pitch = Mathf.Min(sellSound.pitch * pitchMultiplier, maxPitch);
            sellSound.Play();

            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }
            resetCoroutine = StartCoroutine(ResetPitchAfterDelay());
        }
    }

    private IEnumerator ResetPitchAfterDelay()
    {
        yield return new WaitForSeconds(resetTime);
        sellSound.pitch = basePitch; 
    }
}
