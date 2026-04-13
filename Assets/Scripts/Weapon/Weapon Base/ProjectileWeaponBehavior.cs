using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Base script for all projectile weapon behavior (place on prefab on a projectile weapon)
/// </summary>

public class ProjectileWeaponBehavior : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;

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
}
