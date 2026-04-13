using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathBehavior : ProjectileWeaponBehavior
{

    FireBreathController fc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        fc = FindObjectOfType<FireBreathController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * fc.speed * Time.deltaTime; // set the movement of the breath
    }
}
