using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameCommand : Command
{
    public ExitGameCommand() : base()
    {
        this.CommandName = "ExitGame";

    }

    public override void Execute(GameObject gc)
    {
        var Target = gc.GetComponent<Player>();
        if (Target is Player)
        {
            Target.ExitGame();
        }

        base.Execute(gc);
    }
}
