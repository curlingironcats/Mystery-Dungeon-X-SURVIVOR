using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpinController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedFireSpin = Instantiate(weaponData.Prefab);

        Vector3 offset = new Vector3(0f, -0.15f, 0f);

        spawnedFireSpin.transform.position = transform.position + offset; // assign position to be the same as object that is parented to the player
        spawnedFireSpin.transform.parent = transform; // so that it spawns below this object
    }

}
