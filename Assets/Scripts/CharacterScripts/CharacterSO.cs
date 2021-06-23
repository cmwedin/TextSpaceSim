using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public struct Atribs { 
    public Stat Strength;
    public Stat Inteligence;
    public Stat Reflex;
    public Stat Vitality;
}

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "TextSpaceSim/Characters/New Character", order = 0)]
public class CharacterSO : ScriptableObject 
{
    [SerializeField] public bool isPlayer { get; set; }
    public Atribs Attributes;
}
