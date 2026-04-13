using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    SpriteRenderer sr;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            enemy.currentMoveSpeed * Time.deltaTime
        ); 
        }

        if (direction.x < 0)
        {
            sr.flipX = false;
        }
        else if (direction.x > 0)
        {
            sr.flipX = true;
        }
    }

    // call from other scripts to create knockback
    public void Knockback(Vector2 velocity, float duration)
    {
        // ignore knockback if duration is greater than 0
        if(knockbackDuration > 0) return;

        // begin knockback
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}