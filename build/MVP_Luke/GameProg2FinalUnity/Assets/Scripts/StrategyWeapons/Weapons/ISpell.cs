using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell : IWeapon
{
   float ManaUsage { get; set; }

    void CastSpell();
}
