using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGameCommand : Command
{
    public RestartGameCommand() : base()
    {
        this.CommandName = "RestartGame";

    }

    public override void Execute(GameObject gc)
    {
        var Target = gc.GetComponent<Player>();
        if (Target is Player)
        {
            Target.RestartGame();
        }

        base.Execute(gc);
    }
}
