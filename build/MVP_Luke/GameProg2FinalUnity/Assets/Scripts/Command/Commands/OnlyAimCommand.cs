using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyAimCommand : Command
{
    public OnlyAimCommand() : base()
    {
        this.CommandName = "OnlyAim";
    }

    public override void Execute(GameObject gc)
    {
        var target = gc.GetComponent<Player>();
        if(target is Player)
        {
            target.OnlyAim();
        }
        base.Execute(gc);
    }
}
