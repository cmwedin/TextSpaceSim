using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public abstract class Stat
{
  //constructors
  public Stat(string name) {
    _name = name;
    Value= 1;
  }
  public Stat(string name, int val) {
    _name = name;
    Value = val;
  }
  //private fields
  [SerializeField]private string _name;
  [SerializeField]private int _value;
  private int _buff;
  //properties
  public string Name { get => _name; }
  public virtual int Value 
    { get => _value + _buff; 
      set { 
        if(value < 1) 
          { UnityEngine.Debug.Log("Cannot set stats to less then one");
            _value = 1; }
        else _value = value; }
    }    
  public virtual int Buff
  { get => _buff;
    set => _buff = value; }
  public float Check(int Threshold) {
    float checkFactor = (float)this.Value / (float)Threshold;
    return checkFactor;
  }
}
