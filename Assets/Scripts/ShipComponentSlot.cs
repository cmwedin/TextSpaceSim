using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum slotSize
{
    XS,S,M,L,XL,UM
}

public class ShipComponentSlot : ScriptableObject
{
    slotSize size;
    float maxPower;
    public ShipComponent contents;
}
