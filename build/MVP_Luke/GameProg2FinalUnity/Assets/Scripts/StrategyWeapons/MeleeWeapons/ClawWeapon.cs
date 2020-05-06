using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeWeaponStates { Start, Swing, Done}

public class ClawWeapon : MonoBehaviour
{
    public float Damage;
    public Vector3 direction { get; set; }
    protected PlayerOrEnemyShot OwnerOfShot;
    [SerializeField]
    protected MeleeWeaponStates state;

    // Start is called before the first frame update
    void Start()
    {
        this.state = MeleeWeaponStates.Start;
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case MeleeWeaponStates.Start:
                break;
            case MeleeWeaponStates.Swing:
                break;
            case MeleeWeaponStates.Done:
                break;
        }
    }

    public virtual void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player" && OwnerOfShot == PlayerOrEnemyShot.Enemy)
        {
            coll.GetComponent<Attacker>().TakeDamage(Damage);
            this.state = MeleeWeaponStates.Done;
        }
        if (coll.gameObject.tag == "Enemy" && OwnerOfShot == PlayerOrEnemyShot.Player)
        {
            coll.GetComponent<Attacker>().TakeDamage(Damage);
            this.state = MeleeWeaponStates.Done;
        }
        if (coll.gameObject.tag == "Wall")
        {
            this.state = MeleeWeaponStates.Done;
        }
    }
}
