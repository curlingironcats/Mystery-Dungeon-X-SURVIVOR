using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverBob : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Bobbing")]
    public float bobAmplitude = 6f;
    public float bobSpeed = 6f;

    [Header("Scaling")]
    public float hoverScaleMultiplier = 1.08f;

    [Header("Smoothing")]
    public float hoverSmoothSpeed = 8f;

    private Vector3 startPos;
    private Vector3 startScale;

    private bool isHovering;
    private float hoverBlend; // 0 = normal, 1 = fully hovered

    void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
    }

    void Update()
    {
        float targetBlend = isHovering ? 1f : 0f;

        // Smoothly fade hover effect in and out
        hoverBlend = Mathf.Lerp(hoverBlend, targetBlend, Time.unscaledDeltaTime * hoverSmoothSpeed);

        // Optional: snap very close values to avoid tiny lingering offsets
        if (Mathf.Abs(hoverBlend - targetBlend) < 0.001f)
            hoverBlend = targetBlend;

        // Bob only as much as the hover is blended in
        float yOffset = Mathf.Sin(Time.unscaledTime * bobSpeed) * bobAmplitude * hoverBlend;
        transform.localPosition = startPos + new Vector3(0f, yOffset, 0f);

        // Smooth scale up/down with the same hover blend
        float scaleMultiplier = Mathf.Lerp(1f, hoverScaleMultiplier, hoverBlend);
        transform.localScale = startScale * scaleMultiplier;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}