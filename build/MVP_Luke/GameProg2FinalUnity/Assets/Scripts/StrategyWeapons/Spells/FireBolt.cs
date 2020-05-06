using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Spell
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
        this.ManaUsage = 15f;
        this.damage = 30;
        this.speed = 60f;
        this.Name = "Fire Bolt";
        if(this.transform.parent == null)
        {
            this.transform.parent = GetComponentInParent<Caster>().transform;
        }
    }
    public override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);
        if (coll.GetComponent<Caster>() && coll.gameObject.tag == "Enemy" && OwnerOfShot == PlayerOrEnemyShot.Player)
        {
            coll.GetComponent<Caster>().UserCondition = Conditions.Burning;
        }
        if (coll.GetComponent<Caster>() && coll.gameObject.tag == "Player" && OwnerOfShot == PlayerOrEnemyShot.Enemy)
        {
            coll.GetComponent<Caster>().UserCondition = Conditions.Burning;
        }
    }
}
