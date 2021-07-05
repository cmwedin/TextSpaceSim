using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
public class InkItemLayer : MonoBehaviour {
    public InkManager Manager;
    public List<Item> ItemDatabase;

    public Item FindItem(string itemName) {
        return ItemDatabase.Find(x => x.Name == itemName);
    }

}