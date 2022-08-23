using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    bool CanBeDamaged { get; set; }
    void takeDamage(float amount);
}
