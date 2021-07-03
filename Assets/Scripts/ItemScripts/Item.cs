using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private string _name;
    public string Name { get => _name;} 
    [SerializeField] public float _weight;
    public float Weight { get => _weight;}
    [SerializeField] private float _value;
    public float Value { get => _value;} 
}
