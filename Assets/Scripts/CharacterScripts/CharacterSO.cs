using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "TextSpaceSim/Characters/New Character", order = 0)]
public class CharacterSO : ScriptableObject, IDamagable 
{
    public bool isDead {get; private set; }
        = false;
    public delegate void DeathAction();
    public event DeathAction OnDeath;

    //!naming convention for attributes is a 3 letter abbreviation in all caps
    public List<Attrib> Attributes = new List<Attrib>(){
        new Attrib("STR"),
        new Attrib("INT"),
        new Attrib("REF"),
        new Attrib("AGI"),
        new Attrib("VIT"),
        new Attrib("PER"),
        new Attrib("CHR"),
        new Attrib("LCK")};
    //!naming convention is one capitalized word
    [SerializeField] public List<Skill> Skills = new List<Skill>(){};
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
    public void RestoreHealth() {
        Health = MaxHealth;
    }
    public void RestoreHealth(float amount) {
        Health += amount;
    }
    // * functions for SO initialization
    
    //! using this will reset all attributes to 4
    public void CreateAttributes() {
        Attributes = new List<Attrib>();
        Attributes.Add(new Attrib("STR"));//strength
        Attributes.Add(new Attrib("INT"));//intelligence
        Attributes.Add(new Attrib("REF"));//reflex
        Attributes.Add(new Attrib("AGI"));//agility
        Attributes.Add(new Attrib("VIT"));//vitality
        Attributes.Add(new Attrib("PER"));//perception
        Attributes.Add(new Attrib("CHR"));//Charisma
        Attributes.Add(new Attrib("LCK"));//luck
    }
    public void CreateSkills() {
        Skills = new List<Skill>();
        //combat skills
        Skills.Add(new Skill(this, "Sidearms", 
            new List<string>(){"REF","AGI"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Rifles", 
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Melee",
            new List<string>(){"STR","AGI"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Heavy Weaponry",
            new List<string>() {"STR","VIT"},
            new List<double>() {3,2}));
        Skills.Add(new Skill(this, "High-Tech",
            new List<string>(){"INT","PER","REF"},
            new List<double>(){3,1.5,.5}));
        Skills.Add(new Skill(this, "Ballistics",
            new List<string>(){"VIT","REF","STR"},
            new List<double>(){2,2,1}));
        Skills.Add(new Skill(this, "Explosives",
            new List<string>(){"PER","INT","REF"},
            new List<double>(){3,1.5,.5}));
        //ship skills
        Skills.Add(new Skill(this, "Piloting",
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Leadership",
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Medical",
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        //crafting skills
        Skills.Add(new Skill(this, "Chemistry",
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Engineering",
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        //social skills
        Skills.Add(new Skill(this, "Rhetoric",
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Charm",
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Intimidation",
            new List<string>(){"PER","REF"},
            new List<double>(){3,2}));
        /*new skill copy paste blueprint
        Skill.Add(new Skill(this, "",
            new List<string>() {},
            new List<double>() {}));
        */
    }
    public void Init() {
        //CreateAttributes();
        CreateSkills();
        _maxHealth = GetAttribValue("VIT")*10 + _bonusHealth;
        _name = this.name;
        isDead = false;
        RestoreHealth();
        UpdateBaseSkillValues();
        foreach (Skill skill in Skills) {
            skill.Init();
        }
    }

    //QOL function to get a stat's value
    public int GetAttribValue(string name)
    {
        Attrib targetAttribute = this.Attributes.Find(x => x.Name == name);
        if (targetAttribute != null) {return targetAttribute.Value;}
        throw new System.Exception($"CharacterSO Error: attribute {name} not found");
    }
    public Attrib GetAttribObject(string name) {
        Attrib targetAttribute = this.Attributes.Find(x => x.Name == name);
        if (targetAttribute != null) {return targetAttribute;}
        throw new System.Exception($"CharacterSO Error: attribute {name} not found");
    }
    public Skill GetSkillObject(string name) {
        Skill targetAttribute = this.Skills.Find(x => x.Name == name);
        if (targetAttribute != null) {return targetAttribute;}
        throw new System.Exception($"CharacterSO Error: skill {name} not found");
    }
    //! rules of thumb for setting the paramaters: attributes with high check thresholds should be weighted more
    public void UpdateBaseSkillValues() {
        foreach (Skill skill in Skills) {
            skill.SetBaseValue();
        }
    }

}
