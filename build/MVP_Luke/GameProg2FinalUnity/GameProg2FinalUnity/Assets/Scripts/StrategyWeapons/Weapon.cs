using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : IWeapon
{
    protected int damage;
    protected string verb, name;

    public int Damage { get { return damage; } set { damage = value; } }
    public string Verb { get { return verb; } set { verb = value; } }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }
    public string Use()
    {
        return string.Format("{0} that {1}", this.verb, this.name);
    }

    public string Use(IDamageable other)
    {
        other.TakeDamage(this.Damage);

        return this.Use();
    }
}
