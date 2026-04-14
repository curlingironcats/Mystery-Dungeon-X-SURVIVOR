using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        playerCollector.radius = player.currentMagnet;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        // check if game object has the ICollectible interface
        if(col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            //Pulling animation
            //Gets the RigidBody2D component on the item
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            //Vector2 pointing from the item to player
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            //Applies force to item in forcedirection with pullspeed
            rb.AddForce(forceDirection * pullSpeed);

            // if it does, call the collect method
            collectible.Collect();
        }
    }
}
