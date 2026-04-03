using UnityEngine;

public class SmoothFlash : MonoBehaviour
{
    public float speed = 5f;
    public float fadeFactor = 0.5f;
    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        sr.color = Color.Lerp(originalColor, originalColor * fadeFactor, t);
    }
}