using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab {get => prefab; private set => prefab = value;}
    // base stats for weapons
    [SerializeField]
    float damage;
    public float Damage {get=> damage; private set => damage = value;}
    [SerializeField]
    float speed;
    public float Speed {get=> speed; private set => speed = value;}
    [SerializeField]
    float cooldownDuration;
    public float CooldownDuration {get=> cooldownDuration; private set => cooldownDuration = value;}
    [SerializeField]
    float currentCooldown;
    public float CurrentCooldown {get=> currentCooldown; private set => currentCooldown = value;}
    [SerializeField]
    int pierce;
    public int Pierce {get=> pierce; private set => pierce = value;}
    [SerializeField]
    AudioClip clip;
    public AudioClip Clip {get=> clip; private set => clip = value;}
    [SerializeField]
    int level; // not meant to be modified in game, only in editor
    public int Level {get=> level; private set => level = value;}
    [SerializeField]
    GameObject nextLevelPrefab; // the prefab of the next level i.e what the object becomes when it levels up
                                // not to be confused with the prefab spawned at next level
    public GameObject NextLevelPrefab {get=> nextLevelPrefab; private set => nextLevelPrefab = value;}

    [SerializeField]
    new string name;
    public string Name {get=>name; private set=> name=value;}

    [SerializeField]
    string description; // what is the description of this weapon? (if weapon is an upgrade, place description of upgrade)
    public string Description {get=>description; private set=>description=value;}

    [SerializeField]
    Sprite icon;
    public Sprite Icon {get=>icon; private set=>icon = value;}

}
