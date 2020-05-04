using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBolt : Spell
{

    void Update()
    {
        switch (this.State)
        {
            case ShotState.Start:
                break;
            case ShotState.Shooting:
                if (this.transform.parent != null)
                    this.transform.parent = null;
                this.transform.position += direction * speed * Time.deltaTime;
                break;
            case ShotState.Done:
                break;

        }
    }

    public override void SetUpWeapon()
    {
        base.SetUpWeapon();
        this.ManaUsage = 5f;
        this.damage = 5;
        this.speed = 30f;
        this.Name = "Ice Bolt";
    }
    public override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);
        if (coll.GetComponent<Caster>() && coll.gameObject.tag == "Enemy" && OwnerOfShot == PlayerOrEnemyShot.Player)
        {
            coll.GetComponent<Caster>().UserCondition = Conditions.Chilled;
        }
        if (coll.GetComponent<Caster>() && coll.gameObject.tag == "Player" && OwnerOfShot == PlayerOrEnemyShot.Enemy)
        {
            coll.GetComponent<Caster>().UserCondition = Conditions.Chilled;
        }
    }
}
