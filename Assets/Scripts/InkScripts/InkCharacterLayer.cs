using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InkCharacterLayer : MonoBehaviour
{
    [SerializeField] List<CharacterSO> CharacterDatabase; //TODO: populate this list automatically
    public InkManager Manager;
    private CharacterSO currentTarget;
    private CharacterSO prevTarget;

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
        else throw new System.Exception("Character load failed, target " + targetName + " not found");
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
        UnityEngine.Debug.Log("Character Layer's current target was killed");
    }
    //mask of CharacterSO.GetAttrib for input validation
    private int GetAttrib(string name) {
        name = name.Trim(); //removes whitespace just to be safe
        if(name.Length != 3) throw new System.Exception("invalid attribute name used, see CharacterSO.Attributes documentation"); //rejects inputs that dont conform to naming convention
        return currentTarget.GetAttrib(name.ToUpper()); //if everything looks good calls the actual get attrib function making sure the name is all caps
    }
    
    public void Init() {
        BindInkExternals();
        foreach (CharacterSO car in CharacterDatabase)
        {
            car.Init();
        }
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
