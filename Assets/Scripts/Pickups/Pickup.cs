using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool hasBeenCollected = false;
    public AudioClip pickupSFX;

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player")) // if it gets too close to the player, destroy it
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
        }
    }
}
