using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaster : IDamageable
{
    float MaxManaPool { get; set; }

    float CurrentManaPool { get; set; }

    void CastSpell(float manaCost);
}
