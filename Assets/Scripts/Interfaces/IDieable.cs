using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDieable
{
    void Die(bool Impact, bool Blood, bool Stamp);
    bool IsDead { get; set; }
}
