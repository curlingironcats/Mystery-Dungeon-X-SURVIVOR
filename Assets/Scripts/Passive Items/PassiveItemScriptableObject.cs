using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="PassiveItemScriptableObject", menuName="ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier{get=>multiplier; private set=> multiplier=value;}
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
