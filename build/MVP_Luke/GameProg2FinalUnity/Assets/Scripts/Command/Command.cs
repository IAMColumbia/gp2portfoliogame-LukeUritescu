using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : ICommand
{
    public string CommandName;

    public Command()
    {
        
    }

    public virtual void Execute(GameObject gc)
    {
        this.Log();
    }

    protected virtual string Log()
    {
        string LogString = string.Format("{0} executed.", CommandName);

#if DEBUG
        Debug.Log(LogString);
#endif

        return LogString;
    }
}
