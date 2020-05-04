using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoltCommand : Command
{
    public IceBoltCommand() : base()
    {
        this.CommandName = "CastIceBolt";
    }

    public override void Execute(GameObject gc)
    {
        var target = gc.GetComponent<Player>();
        if(target is Player)
        {
            target.CastIceBolt();
        }
        base.Execute(gc);
    }
}
