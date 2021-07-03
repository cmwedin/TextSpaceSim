using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : CharacterItem, IUseable<CharacterSO>
{
    public void Use(CharacterSO user) {
        if (this.RemoveFrom(user.Inventory)) {
            //TODO call item effect delegate
            UnityEngine.Debug.Log($"{user.Name} used one {Name}");
        }
    }
}
