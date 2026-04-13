using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    // current enemy stats
    public float currentMoveSpeed;
    public float currentHealth;
    public float currentDamage;
    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1); // color of damage flash 
    public float damageFlashDuration = 0.2f; // how long the flash lasts
    public float deathFadeTime = 0.33f; // how much time for the enemy to fade after death
    Color originalColor;
    EnemyMovement movement;
    SpriteRenderer sr;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(float damage, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        AudioSource.PlayClipAtPoint(enemyData.Clip, transform.position);
        currentHealth -= damage;
        StartCoroutine(DamageFlash());

        // apply knockback if it is not zero
        if (knockbackForce > 0)
        {
            // get direction of knockback
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    // coroutine that makes the enemy flash when taking damage
    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    public void Kill()
    {
        StartCoroutine(KillFade());
    }

    // coroutine that fades the enemy away slowly
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

    private void OnCollisionStay2D(Collision2D col)
    {
        // reference the script from the colliding object and deal damage using TakeDamage()
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage); // use current damage instead of weapondamage in case multipliers change
        }
    }
}
