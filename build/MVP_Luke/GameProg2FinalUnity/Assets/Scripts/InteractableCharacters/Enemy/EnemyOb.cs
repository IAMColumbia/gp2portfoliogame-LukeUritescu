using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOb : IObserver
{
    EnemyStates state;
    public EnemyStates State
    {
        get { return state; }
        set
        {
            if (this.state != value)        //only set value if state is changed
            {
                this.state = value;
            }
        }
    }
    public EnemyOb(PacMan pac)
    {
        State = EnemyStates.Patrol;
        pac.Attach(this);
    }

    public EnemyOb()
    {
        State = EnemyStates.Patrol;

    }

    public void Attach(PacMan pac)
    {
        pac.Attach(this);
    }

    public void Detach(PacMan pac)
    {
        pac.Detach(this);
    }

    //From IObservable messages from PacMan
    public void ObserverUpdate(object sender, object message)
    {
        if (sender is PacMan)
        {
            
        }
    }
}
