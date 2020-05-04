using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class Caster : MonoBehaviour, ICaster
{
    public float TimerForConditionsToMeet;
    protected float timerToMeetConditionLength;
    protected float tickForDamageOverTime;
    public float DamageOverTimeTimer;

    [SerializeField]
    protected float MaxMovementSpeed;
    [SerializeField]
    protected float movementSpeed;



    public float MaxManaPool { get; set; }

    [SerializeField]
    public float CurrentHP { get; set; }

    public float CurrentManaPool { get; set; }
    public float MaxHP { get; set; }
    public Conditions UserCondition { get; set; }
    public PlayerOrEnemyShot OwnerOfShot { get; set; }


    public virtual void CastSpell(float manaCost)
    {
        CurrentManaPool = CurrentManaPool - manaCost;
    }

    public virtual void TakeDamage(float DamageAmount)
    {
        CurrentHP = CurrentHP - DamageAmount;
    }

    public virtual void SetUpCharacterStats()
    {
        CurrentHP = MaxHP;
        CurrentManaPool = MaxManaPool;
        UserCondition = Conditions.NoCondition;
        timerToMeetConditionLength = 0f;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        tickForDamageOverTime = 0f;
        DamageOverTimeTimer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (UserCondition)
        {
            case Conditions.NoCondition:
                break;
            case Conditions.Burning:
                this.tickForDamageOverTime += Time.deltaTime;
                timerToMeetConditionLength += Time.deltaTime;
                Burning();
                break;
            case Conditions.Chilled:
                this.tickForDamageOverTime += Time.deltaTime;
                timerToMeetConditionLength += Time.deltaTime;
                Chilled();
                break;
        }
    }


    /// <summary>
    /// This should not be implemented in the Caster class because for those who are not casters they can still get burned or chilled
    /// This will need to change in the future 
    /// </summary>

    public void Burning()
    {
        if (timerToMeetConditionLength >= TimerForConditionsToMeet)
        {
            this.UserCondition = Conditions.NoCondition;
            this.timerToMeetConditionLength = 0f;
        }
        if(this.tickForDamageOverTime >= DamageOverTimeTimer)
        {
            this.TakeDamage(2f);
            tickForDamageOverTime = 0f;
        }
    }

    public void Chilled()
    {
        if (timerToMeetConditionLength >= TimerForConditionsToMeet)
        {
            this.UserCondition = Conditions.NoCondition;
            this.timerToMeetConditionLength = 0f;
        }
        if(tickForDamageOverTime >= DamageOverTimeTimer)
        {
            this.movementSpeed /= 2f;
            tickForDamageOverTime = 0f;
        }
    }
}
