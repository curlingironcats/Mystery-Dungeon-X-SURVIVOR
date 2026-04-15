using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedFire = Instantiate(weaponData.Prefab);
        spawnedFire.transform.position = transform.position; // assign position to be the same as parent player object
    }
}
