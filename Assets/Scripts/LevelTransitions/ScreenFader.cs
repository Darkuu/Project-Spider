using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;
    private Image fadeImage;
    public float fadeDuration = 1f; 
    public float fadeHoldDuration = 2f; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeToBlack()
    {
        yield return StartCoroutine(Fade(1f));  
        yield return new WaitForSeconds(fadeHoldDuration);
    }

    public IEnumerator FadeFromBlack()
    {
        yield return StartCoroutine(Fade(0f));  
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}