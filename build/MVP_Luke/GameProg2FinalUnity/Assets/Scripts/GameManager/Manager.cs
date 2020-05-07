using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum ManagerState { NoState, RestartGame, ExitGame, BossDefeated, PlayerDefeated}
public class Manager : IObserver
{

    protected ManagerState managerState;
    public ManagerState State
    {
        get { return managerState; }
        set
        {
            if(this.managerState != value)
            {
                this.managerState = value;
            }
        }
    }

    public Manager(PlayerSubject sub)
    {
        this.State = ManagerState.NoState;
        sub.Attach(this);
    }

    public Manager()
    {
        this.State = ManagerState.NoState;
    }

    public void Attach(PlayerSubject sub)
    {
        sub.Attach(this);
    }

    public void Attach(EnemySub sub)
    {
        sub.Attach(this);
    }

    public void Detach(PlayerSubject sub)
    {
        sub.Detach(this);
    }

    public virtual void Log(string s)
    {
        Debug.Log(s);
    }
    public void ObserverUpdate(object sender, object message)
    {
        if(sender is PlayerSubject)
        {
            if(message is string)
            {
                switch (message.ToString())
                {
                    case "ExitGame":
                        this.State = ManagerState.ExitGame;
                        break;
                    case "RestartGame":
                        this.State = ManagerState.RestartGame;
                        break;
                    case "PlayerDead":
                        this.State = ManagerState.PlayerDefeated;
                        break;
                }
            }
        }
        if(sender is EnemySub)
        {
            if(message is string)
            {
                if(message.ToString() == "BossDead")
                {
                    this.State = ManagerState.BossDefeated;
                }
            }
        }
    }
}
