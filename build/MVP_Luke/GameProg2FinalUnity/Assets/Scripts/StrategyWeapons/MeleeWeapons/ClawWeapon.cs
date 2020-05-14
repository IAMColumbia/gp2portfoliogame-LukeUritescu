using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeWeaponStates { Start, Swing, Done}

public class ClawWeapon : MeleeWeapon
{
    public override void SetUpMeleeWeapon()
    {
        this.Damage = 10f;
        this.RangeOfAttack = 5f;
    }
}
