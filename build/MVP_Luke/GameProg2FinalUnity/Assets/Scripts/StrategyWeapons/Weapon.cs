using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    public virtual int Damage { get; set; }
    public virtual string Name { get; set; }
    public virtual float Speed { get; set; }
    public virtual Vector3 Direction { get; set; }
    public virtual Vector3 Velocity { get; set; }

    public virtual void OnTriggerEnter(Collision trigger)
    {
        if(this.tag != trigger.transform.tag)
        {
            
        }
    }

    public virtual void SetUp()
    {
        
    }

    public void SetUpWeapon()
    {
        throw new System.NotImplementedException();
    }

    public void Use(Transform user)
    {
        throw new System.NotImplementedException();
    }
}
