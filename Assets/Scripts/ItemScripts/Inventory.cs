using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//* extension of dictionary to make it serializable in the editor
//* will move this to its own file if i end up using it elsewhere
//* or the class starts to get to complicated

[System.Serializable] public class ItemQtyDict : SerializableDictionary<Item,int> {}

//* and now the actual class
//? this could probably be a struct
[System.Serializable] public class Inventory
{
    public Inventory(ScriptableObject _owner) {
        Owner = _owner;
    }
    public ScriptableObject Owner; 
    [SerializeField] public ItemQtyDict contents = new ItemQtyDict();
}