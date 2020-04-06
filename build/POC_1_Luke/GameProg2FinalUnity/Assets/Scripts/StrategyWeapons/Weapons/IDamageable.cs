using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int HP
    {
        get;
        set;
    }

    void TakeDamage(int DamageAmount);
}
