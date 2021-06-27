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

    //naming convention for attributes is a 3 letter abreviation in all caps
    public List<Stat> Attributes = new List<Stat>()
        {new Stat("STR"),
         new Stat("INT"),
         new Stat("REF"),
         new Stat("VIT")};
    private float _bonusHealth = 1;
    private float _health;
    public float maxHealth { get => GetAttrib("VIT")*10 + _bonusHealth;}
    [SerializeField] public float Health 
        { get => _health; 
          set { 
            if(isDead) 
                { throw new System.Exception("You cannot change the health of a dead character, use Revive Method (not yet implementend)");}
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

    //QOL function to set a stat's value
    public int GetAttrib(string name) {
        return this.Attributes.Find(x =>x.Name == name).Value;
    }

    //Callback functions
    public void Init() {
        isDead = false;
        Health = maxHealth;
        
    }
}
