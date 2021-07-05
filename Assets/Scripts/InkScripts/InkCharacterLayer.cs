using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InkCharacterLayer : MonoBehaviour
{
    //in principle this should be a hash set but Find is a very usefull method for getting the correct SO from the string name
    [SerializeField] List<CharacterSO> CharacterDatabase; //TODO: populate this list automatically
    public InkManager Manager;
    private CharacterSO _currentTarget;
    private CharacterSO _prevTarget;
    [SerializeField]private PlayerSO _currentPlayer; //TODO: make a save and load system that assigns the current player automatically
    public CharacterSO CurrentTarget { get => _currentTarget;}
    public CharacterSO PrevTarget {get => _prevTarget;}
    public PlayerSO CurrentPlayer { get => _currentPlayer;}
    //callback functions
    void Awake () {
    }
    //custom Methods
    void LoadCharacterSO(string targetName) {
        CharacterSO targ = GetCharacterSOByName(targetName);
        if(targ != null) { 
            if (_prevTarget != null) ClearTargetEvents();
            _prevTarget = _currentTarget;
            _currentTarget = targ;
            SetTargetEvents(); }
        else throw new System.Exception($"Character load failed, target {targetName} not found");
    }
    //potentially doesnt need to be a function
    private CharacterSO GetCharacterSOByName(string name) {
        return CharacterDatabase.Find(x => x.name == name);
    }
    private void ClearTargetEvents() {
        _currentTarget.OnDeath -= TargetKilled;
    }
    private void SetTargetEvents() {
        _currentTarget.OnDeath += TargetKilled;
    }
    private void TargetKilled() {
        UnityEngine.Debug.Log($"Character Layer's current target {_currentTarget.Name} was killed");
    }
    //CharacterSO.GetAttrib handles bad inputs on its own so this isn't really needed 
    //but it allows for some leniency in the inputs of the ink layer functions
    private int GetAttrib(string name) {
         
        name = name.Trim(); 
        if(name.Length != 3) throw new System.Exception($"{name} is not a valid attribute name, see CharacterSO.Attributes documentation"); 
        return _currentTarget.GetAttribValue(name.ToUpper());
    }

    //! Seperation of concerns might mean these functions should be moved to ItemLayer
    //! but since these work with CharacterSO's and CharacterItems I'm keeping them here for now
    //TODO reevaluate where they should be in the future
    private void PickUp(string itemName, CharacterSO charToGive = null, int qty = 1) {
        charToGive = charToGive ?? _currentPlayer;
        Item targetItem = Manager.ItemLayer.FindItem(itemName);
    }
    private void TakeFrom(string itemName, CharacterSO charToTake = null, CharacterSO charToGive = null, int qty = 1) {
        charToTake = charToTake ?? _currentTarget;
        charToGive = charToGive ?? _currentPlayer;
        Item targetItem = Manager.ItemLayer.FindItem(itemName);
        if (targetItem.RemoveFrom(charToTake.Inventory, qty)) {
            targetItem.GiveTo(charToGive.Inventory,qty);
        }
        return;
    }
    
    public void Init() {
        BindInkExternals();
    }
    private void BindInkExternals() {
        //binds ink external functions relevant to the class
        Manager.story.BindExternalFunction("LoadCharacter", (string name) => LoadCharacterSO(name));
        Manager.story.BindExternalFunction("GetAttrib", (string name) => GetAttrib(name));
        Manager.story.BindExternalFunction("GetHealth", () => _currentTarget.Health);
        Manager.story.BindExternalFunction("GetName", () => _currentTarget.name);
        Manager.story.BindExternalFunction("Damage", (float dmg) => _currentTarget.Damage(dmg));
        Manager.story.BindExternalFunction("IsDead", () => _currentTarget.isDead);
    }

}
