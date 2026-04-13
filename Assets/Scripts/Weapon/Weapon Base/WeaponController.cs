using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Base script for all weapon controllers
/// </summary>

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    protected PlayerMovement pm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration; // at the start, set the current cooldown to be the cooldown duration
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f) // once the cooldown becomes 0, attack
        {
            AudioSource.PlayClipAtPoint(weaponData.Clip, transform.position);
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
