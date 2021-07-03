using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//? this could probably be a struct
public class Inventory
{
    public Inventory(ScriptableObject _owner) {
        Owner = _owner;
    }
    public ScriptableObject Owner;
    public Dictionary<Item, int> contents = new Dictionary<Item, int>();
}