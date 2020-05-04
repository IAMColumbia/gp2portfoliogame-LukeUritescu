using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCommand : Command
{
    public MovingCommand() : base()
    {
        this.CommandName = "Moving";
        
    }

    public override void Execute(GameObject gc)
    {
        var target = gc.GetComponent<Player>();
        if(target is Player)
        {
            target.Moving();
        }
        base.Execute(gc);
    }
}
