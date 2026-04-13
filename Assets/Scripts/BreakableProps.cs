using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float health;
    public AudioClip Clip;
    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1); // color of damage flash 
    public float damageFlashDuration = 0.2f; // how long the flash lasts
    public float deathFadeTime = 0.33f; // how much time for the object to fade after death
    [Header("Shake Settings")]
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.1f;
    Color originalColor;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(float damage)
    {
        AudioSource.PlayClipAtPoint(Clip, transform.position);
        health -= damage;
        StartCoroutine(DamageFlash());
        StartCoroutine(Shake());

        // get direction of knockback
        

        if (health <= 0)
        {
            Kill();
        }
    }

    // coroutine that makes the object flash when taking damage
    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    // coroutine the makes the object shak when taking damage
    IEnumerator Shake()
    {
    Vector3 originalPosition = transform.localPosition;

    float elapsed = 0f;

    while (elapsed < shakeDuration)
    {
        float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
        transform.localPosition = originalPosition + new Vector3(offsetX, 0, 0);

        elapsed += Time.deltaTime;
        yield return null;
    }

    transform.localPosition = originalPosition;
    }

    public void Kill()
    {
        StartCoroutine(KillFade());
    }

    // coroutine that fades the object away slowly
    IEnumerator KillFade()
    {
        // waits for a single frame
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.color.a;

        // a loop that fires every frame
        while(t < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;

            // set the color for this frame
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1-t / deathFadeTime) * origAlpha);

        }
        Destroy(gameObject);
    }
}
