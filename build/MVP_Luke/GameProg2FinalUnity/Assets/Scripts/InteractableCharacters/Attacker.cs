using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attacker : MonoBehaviour, IDamageable
{

    public float TimerForConditionsToMeet;
    protected float timerToMeetConditionLength;
    protected float tickForDamageOverTime;
    public float DamageOverTimeTimer;

    public float SpawnTime;
    protected float lastSpawnTime;

    [SerializeField]
    protected float MaxMovementSpeed;
    [SerializeField]
    protected float movementSpeed;
    public float CurrentHP { get; set; }
    public float MaxHP { get; set; }
    public Conditions UserCondition { get; set; }
    public PlayerOrEnemyShot OwnerOfShot { get; set; }


    public virtual void Burning()
    {
        if (timerToMeetConditionLength >= TimerForConditionsToMeet)
        {
            this.UserCondition = Conditions.NoCondition;
            this.timerToMeetConditionLength = 0f;
        }
        if (this.tickForDamageOverTime >= DamageOverTimeTimer)
        {
            this.TakeDamage(10f);
            tickForDamageOverTime = 0f;
        }
    }

    public virtual void Chilled()
    {
        if (timerToMeetConditionLength >= TimerForConditionsToMeet)
        {
            this.UserCondition = Conditions.NoCondition;
            this.timerToMeetConditionLength = 0f;
        }
        if (tickForDamageOverTime >= DamageOverTimeTimer)
        {
            this.movementSpeed /= 2f;
            tickForDamageOverTime = 0f;
        }
    }

    public virtual void TakeDamage(float DamageAmount)
    {
        CurrentHP = CurrentHP - DamageAmount;
    }


    //Have a timer for when it is allowed to attack
    public virtual void MeleeAttack()
    {
        if(lastSpawnTime >= SpawnTime)
        {
            this.GetComponentInChildren<ClawWeapon>().enabled = true;
            lastSpawnTime = 0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.lastSpawnTime = SpawnTime;
        timerToMeetConditionLength = 0f;
        tickForDamageOverTime = 0f;
        DamageOverTimeTimer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        lastSpawnTime += Time.deltaTime;
    }

}
