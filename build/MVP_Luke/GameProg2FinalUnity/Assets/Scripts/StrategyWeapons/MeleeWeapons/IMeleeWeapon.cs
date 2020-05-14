using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeleeWeapon
{
    float RangeOfAttack { get; set; }

    float Damage { get; set; }


    float DealDamage();
}
