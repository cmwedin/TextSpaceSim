using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attrib : Stat
{
    public Attrib(string name) : base(name) {
        Value = 4;
    }
    public Attrib(string name, int val) : base(name, val) {}

    public override int Value 
        { get => base.Value; 
          set { 
                if(Value>10) {
                    UnityEngine.Debug.Log("Cannot set attributes higher than ten");
                    Value = 10; }
              base.Value = value; }
        }
}
    
