using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public class Skill : Stat
{
    private int _defaultValue = 25;
    private int _baseValue;
    public Skill(string name) : base(name){
        Value = 10;
    }
    public Skill(string name, int val) : base(name, val) {}
    public override int Value { 
        get => base.Value; 
        set { 
            if(Value>100) {
                UnityEngine.Debug.Log("Cannot set skills higher than 100");
                Value = 100; }
            base.Value = value; }
    }
    public void SetBaseValue(Attrib attrib, int checkThreshold) {
        _baseValue = (int)(_defaultValue * attrib.Check(checkThreshold));
    }
    //! both lists must be in the same order, the Vec2Ints must be of the form (checkThreshold, Weight) where all Weights add to one
    public void SetBaseValue(List<Attrib> attribs, List<Vector2> paramaters) {
        //Validates inputs while creating convenience variable
        if (attribs.Count != paramaters.Count) { throw new System.Exception("Cannot have more Paramaters than attributes");}
        float weightSum = 0;
        foreach (Vector2 param in paramaters) {
            int checkThreshold = (int)param[0];
            if(checkThreshold < 1 || checkThreshold > 10) 
                { throw new System.Exception($"checkThreshold must be possible value for {attribs[0].Name}");}
            weightSum += param[1];    
        }
        if (weightSum != 1) { throw new System.Exception("Sums of paramater weights must be one");}
        //sets skills value
        float totalCheckFactor = 0;
        for (int _ = 0; _ < attribs.Count; _++) {
            float checkFactor = attribs[_].Check((int)paramaters[_][0]);
            totalCheckFactor +=  checkFactor * paramaters[_][1]; 
        }
        UnityEngine.Debug.Log($"{Name} CheckFactor is {totalCheckFactor}");
        _baseValue = (int)(_defaultValue * totalCheckFactor);
    }
    public void Init() {
        if(_baseValue == 0) {
           Value = _defaultValue;
           UnityEngine.Debug.LogWarning($"Base value of {Name} is never set");}
        else Value = _baseValue;
    }
    
}