using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    private int _value;
    private int _buff;
    public int Value 
        { get => _value + _buff; 
          set => _value = value; }
}
