using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public Image overlayImage;
    public float cycleDuration = 10f; 
    public Color dayColor = new Color(0f, 0f, 0f, 0f);
    public Color nightColor = new Color(0f, 0f, 0f, 0.4f); 

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.PingPong(timer / (cycleDuration / 2f), 1f);
        overlayImage.color = Color.Lerp(dayColor, nightColor, t);
    }
}
