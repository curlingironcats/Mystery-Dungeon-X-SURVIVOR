using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{

    public AudioClip pickupSFX;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        // check if game object has the ICollectible interface
        if(col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
            // if it does, call the collect method
            collectible.Collect();
        }
    }
}
