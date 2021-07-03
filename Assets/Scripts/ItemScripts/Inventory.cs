using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//? this could probably be a struct
public class Inventory
{
    public Dictionary<Item, int> contents = new Dictionary<Item, int>();
}