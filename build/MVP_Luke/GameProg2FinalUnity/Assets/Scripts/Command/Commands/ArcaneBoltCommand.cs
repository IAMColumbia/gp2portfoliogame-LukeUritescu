using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBoltCommand : Command
{

    public ArcaneBoltCommand() : base()
    {
        this.CommandName = "CastArcaneBolt";
    }

    public override void Execute(GameObject gc)
    {
        var target = gc.GetComponent<Player>();
        if(target is Player)
        {
            target.CastArcaneBolt();
        }
        base.Execute(gc);
    }
}
