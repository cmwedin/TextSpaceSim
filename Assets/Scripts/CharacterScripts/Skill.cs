using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable] public class Skill : Stat
{
    private int _defaultValue = 25;
    private int _baseValue;
    public List<Attrib> governingAttributes = new List<Attrib>(){};
    public Dictionary<Attrib, double> governingAttributesWeightDict = new Dictionary<Attrib, double>();
    public Skill(CharacterSO character,string name, List<string> attribNames, List<double> weights) : base(name) {
        // * we have to use a list of string names instead of a string of attributes because the attribute objects are accessed by a function call in CharacterSO
        //* doing that here makes calling the constructor more readable
        if(weights.Sum() != 5 || weights.Count != attribNames.Count) {
            throw new System.Exception($"Error constructing Skill {name} - attribute weights invalid");
        } else { //using the else is probably not needed because of the exception but better safe then sorry
            foreach (string _name in attribNames) {
                governingAttributes.Add(item: character.GetAttribObject(_name));
            }
            for (int _ = 0; _ < governingAttributes.Count; _++) {
                governingAttributesWeightDict.Add(governingAttributes[_],weights[_]);
            SetBaseValue();
            }
        }
    }
    public void SetBaseValue() {
        double doubleSum = 0;
        foreach (KeyValuePair<Attrib, double> attribParam in governingAttributesWeightDict) {
            Attrib attrib = attribParam.Key;
            double weight = attribParam.Value;
            doubleSum += (attrib.Value * weight);
        }
        _baseValue = (int)doubleSum;
    }
/*  //*outdated code currently kept for reference, initialize base values with the above method   
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
    } */
    public void Init() {
        if(_baseValue == 0) {
           Value = _defaultValue;
           UnityEngine.Debug.LogWarning($"Base value of {Name} is never set");}
        else Value = _baseValue;
    }
    
}