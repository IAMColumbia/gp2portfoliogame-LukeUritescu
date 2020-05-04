using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    int Damage { get; set; }

    float Speed { get; set; }

    //Vector3 Direction { get; set; }

    //Vector3 Velocity { get; set; }
    string Name { get; set; }
    //I figure in this style of game, regardless is someone is attacking with a weapon or casting a spell, the weapon needs to know the Direction and Position when used
    void Use(Transform user);

    void SetUpWeapon();
}
