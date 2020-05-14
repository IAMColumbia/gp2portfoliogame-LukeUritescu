using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoltSpawner : SpellSpawner
{
    // Update is called once per frame
    void Update()
    {
        base.addDeadSpellsToRemoveList();
        base.removeObjectInListToRemove();
    }

    public void SpawnTheObject(GameObject go)
    {
        this.Spawn(go);
    }
}
