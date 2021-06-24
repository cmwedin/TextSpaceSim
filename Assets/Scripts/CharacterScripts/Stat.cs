using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Stat
{
    public Stat(string name) {
        _name = name;
        _value = 1;
    }
    private string _name;
    public string Name { get => _name; }
    [SerializeField] private int _value;
    private int _buff;
    
    [SerializeField] public int Value 
        { get => _value + _buff; 
          set { 
            if(value < 0) 
              { UnityEngine.Debug.Log("Cannot set stats to negative");
               _value = 0; }
            else _value = value; }
        }    
        public int Buff
        { get => _buff;
          set => _buff = value; }
}
