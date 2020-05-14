using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour, IMeleeWeapon
{
    public float RangeOfAttack { get; set; }
    public float Damage { get; set; }

    public float DealDamage()
    {
        return this.Damage;
    }

    public virtual void SetUpMeleeWeapon()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        this.Damage = 10f;
        this.RangeOfAttack = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
