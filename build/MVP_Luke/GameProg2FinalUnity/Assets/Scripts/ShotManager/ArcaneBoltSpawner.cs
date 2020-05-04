using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBoltSpawner : SpellSpawner
{
   
    // Update is called once per frame
    void Update()
    {
        base.addDeadSpellsToRemoveList();
        base.removeObjectInListToRemove();
    }



    public void SpawnTheObject()
    {
        this.Spawn();
    }
}
