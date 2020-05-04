using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoltCommand : Command
{
    public FireBoltCommand() : base()
    {
        this.CommandName = "CastFireBolt";

    }

    public override void Execute(GameObject gc)
    {
        var Target = gc.GetComponent<Player>();
        if (Target is Player)
        {
            Target.CastFireBolt();
        }

        base.Execute(gc);
    }
}
