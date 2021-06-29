﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*[System.Serializable] public struct Atribs { 
    public Stat Strength
    public Stat Inteligence;
    public Stat Reflex;
    public Stat Vitality;
}*/

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "TextSpaceSim/Characters/New Character", order = 0)]
public class CharacterSO : ScriptableObject, IDamagable 
{
    public bool isDead {get; private set; }
        = false;
    public delegate void DeathAction();
    public event DeathAction OnDeath;

    //will probably rework these into scriptable objects to simplify extension but this works for now
    //naming convention for attributes is a 3 letter abreviation in all caps
    public List<Attrib> Attributes = new List<Attrib>() {
        new Attrib("STR"),
        new Attrib("INT"),
        new Attrib("REF"),
        new Attrib("VIT")
    };
    //naming convention is one capitalized word
    //will probably stick with these skills regardless of refactoring so this is mostly just for future reference
    [SerializeField] public List<Skill> Skills = new List<Skill>() {
        //combat skills
        new Skill("Sidearms"),
        new Skill("Rifles"),
        new Skill("Melee"),
        new Skill("High-Tech"),
        new Skill("Ballistics"),
        new Skill("Explosives"),
        //ship skills
        new Skill("Piloting"),
        new Skill("Leadership"),
        new Skill("Medical"),
        //crafting skills
        new Skill("Chemistry"),
        new Skill("Engineering"),
        //social skills
        new Skill("Rhetoric"),
        new Skill("Charm"),
        new Skill("Intimidation"),
    };
    private float _bonusHealth = 1;
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private string _name;
    //Read Only values
    public string Name { get => _name;}
    [SerializeField]public float MaxHealth { get => _maxHealth;}
    public float Health 
        { get => _health; 
          set { 
            if(isDead) 
                { throw new System.Exception("CharacterSO Error: You cannot change the health of a dead character, use Revive Method (not yet implementend)");}
            if(value <= 0) 
                Kill();
            else _health = value;} 
        }

    public void Damage(float dmg) {
        float newHealth = Health - dmg;
        //if(newHealth <= 0) UnityEngine.Debug.Log("Character Died");
        Health = newHealth;
    }
    public void Kill() {
        _health = 0;
        isDead = true;
        OnDeath?.Invoke();
    }

    //QOL function to get a stat's value
    public int GetAttrib(string name)
    {
        Attrib targetAttribute = this.Attributes.Find(x => x.Name == name);
        if (targetAttribute != null) {return targetAttribute.Value;}
        throw new System.Exception($"CharacterSO Error: attribute {name} not found");
    }
    public void Init() {
        _maxHealth = GetAttrib("VIT")*10+_bonusHealth;
        _name = this.name;
        isDead = false;
        Health = MaxHealth;
    }
}
