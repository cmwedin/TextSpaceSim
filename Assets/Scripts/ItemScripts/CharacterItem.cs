using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterItem : Item
{
    //! its possible this violates LSP - i think its fine but double check best practices at some point
    public override bool GiveTo(Inventory inventory, int qty = 1) {
        if(inventory.Owner is CharacterSO) {
            return base.GiveTo(inventory,qty);
        } else {
            UnityEngine.Debug.LogWarning("You can only add CharacterItem's to an inventory belonging to a CharacterSO");
            return false;
        }
    }
}
