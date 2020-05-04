using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ShotSprite, ISpell
{
    public float ManaUsage { get; set; }
    public int Damage { get; set; }
    public float Speed { get; set; }
    //public Vector3 Velocity { get; set; }
    public string Name { get; set; }
    protected void Start()
    {
        SetUpWeapon();
    }


    public void CastSpell()
    {

    }

    public virtual void SetUpWeapon()
    {
        this.direction = this.GetComponentInParent<Caster>().transform.forward;
        this.OwnerOfShot = this.GetComponentInParent<Caster>().OwnerOfShot;
        this.State = ShotState.Shooting;
    }

    public void Use(Transform user)
    {
        this.transform.position = user.position;
        this.direction = user.transform.forward;
    }

}
