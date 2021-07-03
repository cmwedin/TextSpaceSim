using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private string _name;
    public string Name { get => _name;} 
    [SerializeField] public float _weight;
    public float Weight { get => _weight;}
    [SerializeField] private float _value;
    public float Value { get => _value;} 
    public virtual bool GiveTo(Inventory inventory, int qty = 1) {
        inventory.contents.Add(this,qty);
        return true;
    }
    public bool RemoveFrom(Inventory inventory, int qtyToRemove = 1) {
        if (inventory.contents.TryGetValue(this, out int targetQty)) {
            targetQty -= qtyToRemove;
            if(targetQty < 0) {
                UnityEngine.Debug.LogWarning($"target inventory does not contain {qtyToRemove} {Name} ");
                return false;
            } else if (targetQty == 0) {
                return inventory.contents.Remove(this);
            } else {
                inventory.contents[this] = targetQty;
                return true;
            }
        }    
        UnityEngine.Debug.LogWarning($"target inventory does not contain item {Name} to remove");
        return false;
    }
}
