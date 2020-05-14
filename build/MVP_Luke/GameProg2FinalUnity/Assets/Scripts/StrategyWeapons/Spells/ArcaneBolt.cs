using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArcaneBolt : Spell
{
    public GameObject PoolArcane;

    void Update()
    {
        switch (this.State)
        {
            case ShotState.Start:
                break;
            case ShotState.Shooting:
                //if (this.transform.parent != null)
                //    this.transform.parent = null;
                this.transform.position += direction * speed * Time.deltaTime;
                break;
            case ShotState.Done:
                break;

        }
    }

    public override void SetUpWeapon()
    {
        base.SetUpWeapon();
        this.ManaUsage = 10f;
        this.damage = 10;
        this.speed = 70f;
        this.Name = "Arcane Bolt";
    }
    public override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);
    }
}
