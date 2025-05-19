using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float fadeDuration = 1f;

    private float lifetime;
    private Color originalColor;

    public void SetText(string message, Color color)
    {
        text.text = message;
        originalColor = color;
        text.color = color;
        lifetime = 0f;
    }

    private void Update()
    {
        // Move up
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade out
        lifetime += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0f, lifetime / fadeDuration);
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (lifetime >= fadeDuration)
            Destroy(gameObject);
    }
}
