using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySub : ISubject
{
    List<IObserver> GameManagers;

    public virtual void Log(string s)
    {

    }

    public EnemySub()
    {
        this.GameManagers = new List<IObserver>();
    }

    public void Attach(IObserver o)
    {
        this.GameManagers.Add(o);
    }

    //should only be one

    public void Detach(IObserver o)
    {
        this.GameManagers.Remove(o);
    }

    public void Notify()
    {
        foreach(IObserver o in GameManagers)
        {
            o.ObserverUpdate(this, "Message From Enemy");
        }
    }

    public void Notify(string s)
    {
        foreach (IObserver o in GameManagers)
        {
            o.ObserverUpdate(this, s);
        }
    }

}
