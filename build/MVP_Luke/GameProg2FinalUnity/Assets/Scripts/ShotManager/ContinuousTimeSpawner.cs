using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousTimeSpawner : TimeSpawner
{
    private void Update()
    {
        base.addDeadSpellsToRemoveList();
        base.removeObjectInListToRemove();

        lastSpawnTime += Time.deltaTime;
        if(lastSpawnTime > SpawnTime)
        {
            lastSpawnTime = 0.0f;
            this.Spawn();
        }
    }
}
