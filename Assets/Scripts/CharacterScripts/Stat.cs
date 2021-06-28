using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public abstract class Stat
{
    public Stat(string name) {
        _name = name;
        Value= 1;
    }
    private string _name;
    public string Name { get => _name; }
    [SerializeField] private int _value;
    private int _buff;
    
    [SerializeField] public virtual int Value 
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
}
