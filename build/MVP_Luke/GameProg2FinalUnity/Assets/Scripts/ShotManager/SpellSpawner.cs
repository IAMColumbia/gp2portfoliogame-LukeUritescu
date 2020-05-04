using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : PooledSpawner
{
    public override void SetupSpawnObject(GameObject go)
    {
        base.SetupSpawnObject(go);
        if(go.GetComponent<Spell>() != null)
        {
            Spell gs = go.GetComponent<Spell>();
            gs.gameObject.transform.position = this.transform.position;
            gs.SetUpWeapon();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.removeObjectInListToRemove();
        addDeadSpellsToRemoveList();
    }

    protected void addDeadSpellsToRemoveList()
    {
        Spell gs;
        foreach(GameObject go in this.gameObjects)
        {
            gs = go.GetComponent<Spell>();
            if(go.GetComponent<Spell>() != null)
            {
                if(gs.State == ShotSprite.ShotState.Done)
                {
                    this.objectsToRemove.Add(gs.gameObject);
                }
            }
        }
    }
}
