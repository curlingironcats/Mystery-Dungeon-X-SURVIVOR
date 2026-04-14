using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplePotion : Pickup, ICollectible
{
    public int healthToRestore;

    public void Collect()
    {
        hasBeenCollected = true;
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
    }
}
