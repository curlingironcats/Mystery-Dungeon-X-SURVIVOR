using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D detector;
    public float pullSpeed;

    void Start()
    {
        player = GetComponentInParent<PlayerStats>();
    }

    public void SetRadius(float r)
    {
        if (!detector) detector = GetComponent<CircleCollider2D>();
        detector.radius = r;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        // check if game object is a pickup
        if(col.TryGetComponent(out Pickup collectible))
        {
            collectible.Collect(player, pullSpeed);
        }
    }
}