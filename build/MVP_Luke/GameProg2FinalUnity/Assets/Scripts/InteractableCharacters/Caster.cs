using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class Caster : Attacker, ICaster
{



    public float MaxManaPool { get; set; }
    public float CurrentManaPool { get; set; }


    public virtual void CastSpell(float manaCost)
    {
        CurrentManaPool = CurrentManaPool - manaCost;
    }

    public virtual void SetUpCharacterStats()
    {
        CurrentHP = MaxHP;
        CurrentManaPool = MaxManaPool;
        UserCondition = Conditions.NoCondition;
        
    }

    // Start is called before the first frame update
    void Start()
    {

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
    
}
