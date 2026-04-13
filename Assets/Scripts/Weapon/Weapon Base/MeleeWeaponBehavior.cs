using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Base script for all melee weapon behavior (place on a melee weapon prefab)
/// </summary>

public class MeleeWeaponBehavior : MonoBehaviour
{
    
    public float destroyAfterSeconds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }
    
}
