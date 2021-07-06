using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class ItemQtyDict : SerializableDictionary<Item,int> {}
//? this could probably be a struct
[System.Serializable] public class Inventory
{
    public Inventory(ScriptableObject _owner) {
        Owner = _owner;
    }
    public ScriptableObject Owner; 
    [SerializeField] public ItemQtyDict contents = new ItemQtyDict();
}