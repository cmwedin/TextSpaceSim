using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    float Health { get; }
    void Damage(float dmg);
    void Kill();
}
