using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Knockback")]
    public float knockbackForce = 8f;
    private Rigidbody2D rb;

    public List<LevelRange> levelRanges;

    PlayerCollector collector;

    PlayerMovement movement;

    InventoryManager inventory;
    Color originalColor;
    SpriteRenderer sr;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public Text levelText;
    public AudioClip Clip;

    public GameObject firstPassiveItemTest, secondPassiveItemTest;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1); // color of damage flash 
    public float damageFlashDuration = 0.2f; // how long the flash lasts

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();
        collector = GetComponentInChildren<PlayerCollector>();

        // assign the variables
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;
        collector.SetRadius(characterData.Magnet);

        // spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
        //SpawnPassiveItem(firstPassiveItemTest);
        //SpawnPassiveItem(secondPassiveItemTest);
    }

    void Start()
    {
        // initialize the experience cap as the first experience increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
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

        UpdateExpBar();
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

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LV. " + level.ToString();
    }

    public void TakeDamage(float damage, Vector2 sourcePosition)
    {
        if(!isInvincible)
        {
            AudioSource.PlayClipAtPoint(Clip, transform.position);
            currentHealth -= damage;
            StartCoroutine(DamageFlash());

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            movement.ApplyKnockback(sourcePosition, knockbackForce);

            if(currentHealth <= 0)
            {
                Kill();
            }

            UpdateHealthBar();
        }
    }

    // coroutine that makes the player flash when taking damage
    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    void UpdateHealthBar()
    {
        // update the health bar
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }

    public void Kill()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.GameOver();
        }
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

            UpdateHealthBar();
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

        UpdateHealthBar();
    }

    public void SpawnWeapon(GameObject weapon)
    {
        // check if slots are full; return if they are
        if(weaponIndex >= inventory.weaponSlots.Count -1) // must be -1 because a list starts from 0
        {
            return;
        }

        // spawn starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); // set weapon to be child of player
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>()); // add weapon to its inventory slot

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        // check if slots are full; return if they are
        if(passiveItemIndex >= inventory.passiveItemSlots.Count -1) // must be -1 because a list starts from 0
        {
            return;
        }

        // spawn starting weapon
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); // set weapon to be child of player
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); // add weapon to its inventory slot

        passiveItemIndex++;
    }
}
