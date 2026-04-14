using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Base script for all projectile weapon behavior (place on prefab on a projectile weapon)
/// </summary>

public class ProjectileWeaponBehavior : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;

    // Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().currentMight;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirx < 0 && diry == 0)
        {
            scale.x *= -1;
            scale.y *= -1;
        }
        else if (dirx == 0 && diry < 0) // down
        {
            rotation.z = 0f;
        }
        else if (dirx == 0 && diry > 0) // up
        {
            rotation.z = 180f;
        }
        else if (dir.x > 0 && dir.y > 0) // right up
        {
            rotation.z = 135f;
        }
        else if (dir.x > 0 && dir.y < 0) // right down
        {
            rotation.z = 35f;
        }
        else if (dir.x < 0 && dir.y > 0) // left up
        {
            rotation.z = -135f;
        }
        else if (dir.x < 0 && dir.y < 0) // left down
        {
            rotation.z = -35f;            
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); // can't set vector because cannot convert
    }

    // reference the script from the collider and deal damage using TakeDamage()
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(), transform.position);  // use currentDamage instead of weapondata damage in case current values change
            ReducePierce();
        }
        else if (col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }
    }

    void ReducePierce() // Destroy once the pierce hits 0
    {
        currentPierce--;
        if(currentPierce == 0)
        {
            Destroy(gameObject);
        }
    }
}
