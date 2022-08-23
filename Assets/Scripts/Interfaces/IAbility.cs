using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    void ActivateAbility(Abilities abilities, out bool succeed);
}

public enum Abilities
{
    Defense = 0,
    Protect = 1,
    Attack = 2
}

