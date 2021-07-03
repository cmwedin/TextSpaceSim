using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterItem : Item
{
    //! its possible this violates LSP but i think its fine
    public override bool GiveTo(Inventory inventory, int qty = 1) {
        if(inventory.Owner is CharacterSO) {
            return base.GiveTo(inventory,qty);
        } else {
            UnityEngine.Debug.LogWarning("You can only add CharacterItem's to an inventory belonging to a CharacterSO");
            return false;
        }
    }
}
