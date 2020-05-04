using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Conditions { NoCondition, Burning, Chilled}
public interface IDamageable
{
    float CurrentHP
    {
        get;
        set;
    }

    float MaxHP
    {
        get;set;    
    }

    /// <summary>
    /// Should ask for help on what is the best method to set conditions to Players/Enemies
    /// 
    /// When an object is burning it will receive damage over time, the damage is based on what caused the burn
    /// When an object is chilled it will slow down its movementSpeed
    /// Perhaps a Enum of states is: NoCondition, Burning, Chilled
    /// </summary>
    Conditions UserCondition { get; set; }

    void TakeDamage(float DamageAmount);

    void Burning();
    void Chilled();
    
}
