using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weap : WeaponPickUp, IWeapon
{

    private int damage;
    private string verb, name;

    public int Damage
    {
        get
        {
            return this.damage;
        }

        set
        {
            this.damage = value;
        }
    }
    public string Verb
    {
        get
        {
            return this.verb;
        }
        set
        {
            this.verb = value;
        }
    }

    public string Name
    {
        get
        {
            return this.name;
        }

        set
        {
            this.name = value;
        }
    }

    public string Use()
    {
        throw new System.NotImplementedException();
    }

  
}
