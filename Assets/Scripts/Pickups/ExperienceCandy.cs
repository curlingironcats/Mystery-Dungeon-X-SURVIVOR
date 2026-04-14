using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceCandy : Pickup, ICollectible
{
    
    public int experienceGranted;

    public void Collect()
    {
        hasBeenCollected = true;
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
