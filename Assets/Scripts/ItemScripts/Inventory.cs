using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class ItemQtyDict : SerializableDictionary<Item,int> {
    public override int this[Item key] {
        get { return base[key]; } 
        set { 
            if(value <= 0) {this.Remove(key);}
            else { base[key] = value;} 
        }
    }
}
//? this could probably be a struct
[System.Serializable] public class Inventory
{
    public Inventory(ScriptableObject _owner) {
        Owner = _owner;
        contents = new ItemQtyDict();
    }
    public ScriptableObject Owner; 
    [SerializeField] public ItemQtyDict contents;
}