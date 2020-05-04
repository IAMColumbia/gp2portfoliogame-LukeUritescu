using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpawner : SpellSpawner
{
    public float SpawnTime;
    protected float lastSpawnTime;
    protected bool spawned;

    private void Update()
    {
        base.addDeadSpellsToRemoveList();
        base.removeObjectInListToRemove();

        lastSpawnTime += Time.deltaTime;
        if((lastSpawnTime > SpawnTime) && (spawned == false))
        {
            this.Spawn();
            this.spawned = true;
        }
    }
}
