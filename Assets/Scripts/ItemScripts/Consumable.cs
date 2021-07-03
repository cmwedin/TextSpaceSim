using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "TextSpaceSim/Items/Character Items/Consumables", order = 0)]
public class Consumable : CharacterItem, IUseable<CharacterSO>
{
    public void Use(CharacterSO user) {
        if (this.RemoveFrom(user.Inventory)) {
            //TODO call item effect delegate
            UnityEngine.Debug.Log($"{user.Name} used one {Name}");
        }
    }
}
