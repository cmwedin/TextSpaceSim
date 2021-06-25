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
    public bool isPlayer { get; private set; }

    //naming convention for attributes is a 3 letter abreviation in all caps
    public List<Stat> Attributes = new List<Stat>()
        {new Stat("STR"),
         new Stat("INT"),
         new Stat("REF"),
         new Stat("VIT")};
    private float _bonusHealth = 1;
    private float _health;
    public float maxHealth { get => GetAttrib("VIT")*10 + _bonusHealth;}
    [SerializeField] public float Health { get => _health; }

    public void Damage(float dmg) {
        float newHealth = _health - dmg;
        if(newHealth <= 0) UnityEngine.Debug.Log("Character Died");
        _health = newHealth;
    }

    //QOL function to set a stat's value
    public int GetAttrib(string name) {
        return this.Attributes.Find(x =>x.Name == name).Value;
    }

    //Callback functions
    private void Init() {
        _health = maxHealth;
    }
}
