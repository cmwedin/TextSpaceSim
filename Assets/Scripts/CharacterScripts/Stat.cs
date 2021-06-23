using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Stat
{
    public string Name { get; }
    private int _value;
    private int _buff;
    
    [SerializeField] public int Value 
        { get => _value + _buff; 
          set => _value = value; }
    public int Buff
        { get => _buff;
          set => _buff = value; }
}
