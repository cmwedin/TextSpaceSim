using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Ink.Runtime;
public class InkItemLayer : MonoBehaviour {
    public InkManager Manager;
    public List<Item> ItemDatabase;
    InkList ItemNameDatabase;
    public Item FindItem(string itemName) {
        return ItemDatabase.Find(x => x.Name == itemName);
    }
    public InkList GetItemList(Inventory inventory) {
        var output = new Ink.Runtime.InkList();
        //List<string> itemNames = new List<string>();
        //List<int> itemQtys = new List<int>();
        foreach (Item item in inventory.contents.Keys) {
            InkListItem listEntry = new InkListItem("inventory",item.Name);
            output.Add(listEntry,inventory.contents[item]);
        }
        return output;
    }
    public void Init() {
        ItemNameDatabase = new Ink.Runtime.InkList("ItemNameDatabase", Manager.story);
    }

}