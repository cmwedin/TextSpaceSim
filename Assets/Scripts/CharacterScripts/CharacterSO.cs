using System.Collections;
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
    //naming convention for attributes is a 3 letter abbreviation in all caps
    public List<Attrib> Attributes = new List<Attrib>() {
        new Attrib("STR"),//strength
        new Attrib("INT"),//intelligence
        new Attrib("REF"),//reflex
        new Attrib("VIT"),//vitality
        new Attrib("PER"),//perception
        new Attrib("CHR"),//Charisma
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
        GetSkillObject("Sidearms").SetBaseValue(
            new List<Attrib>() { 
                GetAttribObject("REF"),
                GetAttribObject("PER") 
            }, new List<Vector2>() {
                new Vector2(6,(float)0.60),
                new Vector2(5,(float)0.40)
            });
        GetSkillObject("Rifles").SetBaseValue(
            new List<Attrib>() { 
                GetAttribObject("PER"),
                GetAttribObject("REF") 
            }, new List<Vector2>() {
                new Vector2(6,(float)0.60),
                new Vector2(5,(float)0.40)
            });
        GetSkillObject("Melee").SetBaseValue(
            new List<Attrib>() {
                GetAttribObject("STR"),
                GetAttribObject("VIT"),
                GetAttribObject("REF")
            }, new List<Vector2>() {
                new Vector2(5,(float)0.60),
                new Vector2(4,(float)0.30),
                new Vector2(3,(float)0.10)
            });
        GetSkillObject("High-Tech").SetBaseValue(
            new List<Attrib>() {
                GetAttribObject("INT"),
                GetAttribObject("PER"),
                GetAttribObject("REF")
            }, new List<Vector2>() {
                new Vector2(8,(float)0.60),
                new Vector2(5,(float)0.30),
                new Vector2(2,(float)0.10)
            });        
        GetSkillObject("Ballistics").SetBaseValue(
            new List<Attrib>() { 
                GetAttribObject("REF"),
                GetAttribObject("PER") 
            }, new List<Vector2>() {
                new Vector2(8,(float)0.5),
                new Vector2(8,(float)0.5)
            });
        GetSkillObject("Explosives").SetBaseValue(
            new List<Attrib>() {
                GetAttribObject("PER"),
                GetAttribObject("STR"),
                GetAttribObject("INT")
            }, new List<Vector2>() {
                new Vector2(7,(float)0.60),
                new Vector2(5,(float)0.30),
                new Vector2(5,(float)0.10)
            });
        GetSkillObject("Piloting").SetBaseValue(
            new List<Attrib>() {
                GetAttribObject("REF"),
                GetAttribObject("PER"),
                GetAttribObject("INT")
            }, new List<Vector2>() {
                new Vector2(8,(float)0.60),
                new Vector2(5,(float)0.30),
                new Vector2(2,(float)0.10)
            });
        GetSkillObject("Leadership").SetBaseValue(
            new List<Attrib>() {
                GetAttribObject("CHR"),
                GetAttribObject("INT"),
                GetAttribObject("PER")
            }, new List<Vector2>() {
                new Vector2(8,(float)0.70),
                new Vector2(5,(float)0.20),
                new Vector2(2,(float)0.10)
            });
        GetSkillObject("Medical").SetBaseValue(
            new List<Attrib>() {
                GetAttribObject("INT"),
                GetAttribObject("REF"),
                GetAttribObject("PER"),
                GetAttribObject("CHR")
            }, new List<Vector2>() {
                new Vector2(8,(float)0.60),
                new Vector2(4,(float)0.25),
                new Vector2(4,(float)0.10),
                new Vector2(2,(float)0.05)
            }); 
        GetSkillObject("Chemistry").SetBaseValue(
            new List<Attrib>() { 
                GetAttribObject("INT"),
                GetAttribObject("PER")
            }, new List<Vector2>() {
                new Vector2(6,(float)0.60),
                new Vector2(5,(float)0.40)
            });
        GetSkillObject("Engineering").SetBaseValue(
            new List<Attrib>() { 
                GetAttribObject("INT"),
                GetAttribObject("REF") 
            }, new List<Vector2>() {
                new Vector2(6,(float)0.60),
                new Vector2(5,(float)0.40)
            });
        GetSkillObject("Charm").SetBaseValue(
            new List<Attrib>() { 
                GetAttribObject("CHR"),
                GetAttribObject("PER") 
            }, new List<Vector2>() {
                new Vector2(7,(float)0.60),
                new Vector2(4,(float)0.40)
            });
        GetSkillObject("Rhetoric").SetBaseValue(
            new List<Attrib>() { 
                GetAttribObject("CHR"),
                GetAttribObject("PER") 
            }, new List<Vector2>() {
                new Vector2(7,(float)0.60),
                new Vector2(4,(float)0.40)
            });
        GetSkillObject("Intimidation").SetBaseValue(
            new List<Attrib>() { 
                GetAttribObject("CHR"),
                GetAttribObject("INT") 
            }, new List<Vector2>() {
                new Vector2(7,(float)0.60),
                new Vector2(4,(float)0.40)
            });    
    }
    public void RestoreHealth() {
        Health = MaxHealth;
    }
    public void RestoreHealth(float amount) {
        Health += amount;
    }
    public void Init() {
        _maxHealth = GetAttribValue("VIT")*10 + _bonusHealth;
        _name = this.name;
        isDead = false;
        RestoreHealth();
        UpdateBaseSkillValues();
        foreach (Skill skill in Skills) {
            skill.Init();
        }
    }
}
