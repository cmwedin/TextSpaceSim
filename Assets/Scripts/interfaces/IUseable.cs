using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable<T>
{
    public void Use(T user);
}
