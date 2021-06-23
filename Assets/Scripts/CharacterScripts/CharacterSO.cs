using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct Atribs
{
    public int Strength { get; set; }
    public int Inteligence { get; set; }
    public int Reflex { get; set; }
    public int Vitality { get; set; }
}

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "TextSpaceSim/Characters/New Character", order = 0)]
public class CharacterSO : ScriptableObject 
{
    [SerializeField] public bool isPlayer { get; set; }
}
