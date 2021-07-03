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
    //* character property collections
    //!naming convention for attributes is a 3 letter abbreviation in all caps
    [SerializeField] public List<Attrib> Attributes = new List<Attrib>(){};
    [SerializeField] public List<Skill> Skills = new List<Skill>(){};
    public Inventory Inventory;
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
        Attributes = new List<Attrib>();                    //? after a lot of not especially productive time spent tweaking i think this if fine balance
        Attributes.Add(new Attrib("STR"));//strength        //? 2-3* 1-2* 1-1* total per point 9
        Attributes.Add(new Attrib("FOC"));//focus           //? 1-3* 2-2* 4-1* total per point 11
        Attributes.Add(new Attrib("INT"));//Intelligence    //? 3-3* 1-2* 1-1* total per point 12
        Attributes.Add(new Attrib("REF"));//reflex          //? 1-3* 3-2* 1-1* total per point 10
        Attributes.Add(new Attrib("VIT"));//vitality        //? 1-3* 3-2* 2-1* total per point 10 
        Attributes.Add(new Attrib("PER"));//perception      //? 2-3* 1-2* 2-1*  total per point 11
        Attributes.Add(new Attrib("CHR"));//Charisma        //? 2-3* 2-2* 1-1* total per point 11
        Attributes.Add(new Attrib("LCK"));//luck            //? not used for skills
    }
    public void CreateSkills() {
        Skills = new List<Skill>();
        //combat skills
        Skills.Add(new Skill(this, "Sidearms", 
            new List<string>(){"REF","FOC"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Rifles", 
            new List<string>(){"PER","FOC"},
            new List<double>(){3, 2}));
        Skills.Add(new Skill(this, "Melee",
            new List<string>(){"STR","VIT"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Heavy Weaponry", //? maybe too similar to Melee
            new List<string>() {"VIT","STR"},
            new List<double>() {3,2}));
        Skills.Add(new Skill(this, "High-Tech",
            new List<string>(){"INT","REF","FOC"},
            new List<double>(){2,2,1}));
        Skills.Add(new Skill(this, "Ballistics",
            new List<string>(){"VIT","REF","STR"},
            new List<double>(){2,2,1}));
        Skills.Add(new Skill(this, "Explosives",
            new List<string>(){"PER","FOC","REF"},
            new List<double>(){3,1,1}));
        //ship skills
        Skills.Add(new Skill(this, "Piloting",
            new List<string>(){"PER","REF","VIT"},
            new List<double>(){2,2,1}));
        Skills.Add(new Skill(this, "Leadership",            
            new List<string>(){"CHR","FOC","INT"},
            new List<double>(){3,1,1}));
        Skills.Add(new Skill(this, "Medical", 
            new List<string>(){"INT","FOC","CHR"},
            new List<double>(){3,1,1}));
        //crafting skills
        Skills.Add(new Skill(this, "Chemistry", 
            new List<string>(){"INT","PER"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Engineering",
            new List<string>(){"FOC","VIT"},
            new List<double>(){3,2}));
        //social skills
        Skills.Add(new Skill(this, "Rhetoric", 
            new List<string>(){"INT","CHR"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Charm",
            new List<string>(){"CHR","PER"},
            new List<double>(){3,2}));
        Skills.Add(new Skill(this, "Intimidation",
            new List<string>(){"STR","CHR"},
            new List<double>(){3,2}));
        /*new skill copy paste blueprint
        Skill.Add(new Skill(this, "",
            new List<string>() {},
            new List<double>() {}));
        */
    }
    public void Init() {
        CreateAttributes();
        CreateSkills();
        Inventory = new Inventory(this);
        _maxHealth = GetAttribValue("VIT")*10 + _bonusHealth;
        _name = this.name;
        isDead = false;
        RestoreHealth();
        foreach (Skill skill in Skills) {
            skill.Init();
        }
    }
    // * Look-up functions
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
    public void UpdateBaseSkillValues() {
        foreach (Skill skill in Skills) {
            skill.SetBaseValue();
        }
    }

}
