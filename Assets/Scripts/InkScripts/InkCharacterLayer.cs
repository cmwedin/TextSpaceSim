using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InkCharacterLayer : MonoBehaviour
{
    //in principle this should be a hash set but Find is a very usefull method for getting the correct SO from the string name
    [SerializeField] List<CharacterSO> CharacterDatabase; //TODO: populate this list automatically
    public InkManager Manager;
    private CharacterSO currentTarget;
    private CharacterSO prevTarget;
    [SerializeField]private PlayerSO currentPlayer; //TODO: make a save and load system that assigns the current player automatically

    //callback functions
    void Awake () {
    }
    //custom Methods
    void LoadCharacterSO(string targetName) {
        CharacterSO targ = GetCharacterSOByName(targetName);
        if(targ != null) { 
            if (prevTarget != null) ClearTargetEvents();
            prevTarget = currentTarget;
            currentTarget = targ;
            SetTargetEvents(); }
        else throw new System.Exception($"Character load failed, target {targetName} not found");
    }
    //potentially doesnt need to be a function
    private CharacterSO GetCharacterSOByName(string name) {
        return CharacterDatabase.Find(x => x.name == name);
    }
    private void ClearTargetEvents() {
        currentTarget.OnDeath -= TargetKilled;
    }
    private void SetTargetEvents() {
        currentTarget.OnDeath += TargetKilled;
    }
    private void TargetKilled() {
        UnityEngine.Debug.Log($"Character Layer's current target {currentTarget.Name} was killed");
    }
    //CharacterSO.GetAttrib handles bad inputs on its own so this isnt really needed 
    //but it allows for some leiniency in the inputs of the ink layer functions
    private int GetAttrib(string name) {
         
        name = name.Trim(); 
        if(name.Length != 3) throw new System.Exception($"{name} is not a valid attribute name, see CharacterSO.Attributes documentation"); 
        return currentTarget.GetAttribValue(name.ToUpper());
    }
    
    public void Init() {
        BindInkExternals();
    }
    private void BindInkExternals() {
        //binds ink external functions relevant to the class
        Manager.story.BindExternalFunction("LoadCharacter", (string name) => LoadCharacterSO(name));
        Manager.story.BindExternalFunction("GetAttrib", (string name) => GetAttrib(name));
        Manager.story.BindExternalFunction("GetHealth", () => currentTarget.Health);
        Manager.story.BindExternalFunction("GetName", () => currentTarget.name);
        Manager.story.BindExternalFunction("Damage", (float dmg) => currentTarget.Damage(dmg));
        Manager.story.BindExternalFunction("IsDead", () => currentTarget.isDead);
    }

}
