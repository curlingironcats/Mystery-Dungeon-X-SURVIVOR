using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    // current stats
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;

    // spawned weapon
    public List<GameObject> spawnedWeapons;

    // experience and player level
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    // class for defining a level range and experience cap for that level range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    // I-frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        // assign the variables
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;

        // spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
    }

    void Start()
    {
        // initialize the experience cap as the first experience increase
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        // if the timer has reached zero, set invincibility to false, iframe over
        else if(isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float damage)
    {
        // if player is not currently invincible, reduce health and start invincibility
        if(!isInvincible)
        {
            currentHealth -= damage;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if(currentHealth <= 0)
            {
                Kill();
            }
            
        }
    }

    public void Kill()
    {
        Debug.Log("player is dead");
    }

    public void RestoreHealth(float amount)
    {
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;

            // make sure player's health doesn't exceed max health
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

            // make sure the player's health doesn't exceed max health
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        // spawn starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); // set weapon to be child of player
        spawnedWeapons.Add(spawnedWeapon); // add weapon to list of spawned weapons
    }
}
