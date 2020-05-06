using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : ISubject
{
    List<IObserver> EnemyObs;

    public virtual void Log(string s)
    {
        //nothing
    }

    public PacMan()
    {
        this.EnemyObs = new List<IObserver>();
    }

    public void Attach(IObserver o)
    {
        this.EnemyObs.Add(o);
    }

    public void Detach(IObserver o)
    {
        this.EnemyObs.Remove(o);

        int indexOfFirstDeadEnemy = 0;
        foreach(EnemyOb g in EnemyObs)
        {
            if(g.State == EnemyStates.Dead)
            {
                indexOfFirstDeadEnemy = EnemyObs.IndexOf(g);
                this.EnemyObs.RemoveAt(indexOfFirstDeadEnemy);
            }
        }
    }

    public void Notify()
    {
        foreach(IObserver o in EnemyObs)
        {
            o.ObserverUpdate(this, "Message from PacMan");
        }
    }

    public void Notify(string s)
    {
        foreach(IObserver o in EnemyObs)
        {
            o.ObserverUpdate(this, s);
        }
    }
}
