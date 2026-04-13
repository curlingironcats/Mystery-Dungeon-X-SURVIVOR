using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpinBehavior : MeleeWeaponBehavior
{
    List<GameObject> markedEnemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage, transform.position);

            markedEnemies.Add(col.gameObject);
        }
    }
}
