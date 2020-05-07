using System.Collections;
using System.Collections.Generic;
using UnityCommand;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{
    KeyMap keyMap;

    Stack<ICommand> Commands = new Stack<ICommand>();

    public GameObject MoveCommandTarget;
    public CommandProcessor() : base()
    {
        keyMap = new KeyMap();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var item in keyMap.OnReleasedKeyMap)
        {
            if (Input.GetKeyUp(item.Key))
            {
                Debug.Log(string.Format("onReleasedKeyMap Key Released {0}", item.Value.ToString()));
                //Command command = null;
            }
        }

        foreach(var item in keyMap.OnKeyDownMap)
        {
            if (Input.GetKey(item.Key))
            {
                Command command = null;
                switch (item.Value)
                {
                 case "OnlyAim":
                        command = new OnlyAimCommand();
                        break;
                }
                if (command != null)
                {
                    if (command is ICommand)
                    {
                        Commands.Push((ICommand)command);
                    }
                    command.Execute(MoveCommandTarget);
                }
            }

            if (Input.GetKeyDown(item.Key))
            {
                Command command = null;
                switch (item.Value)
                {
                    case "Moving":
                        command = new MovingCommand();
                        break;
                    case "CastArcaneBolt":
                        command = new ArcaneBoltCommand();
                        break;
                    case "CastFireBolt":
                        command = new FireBoltCommand();
                        break;
                    case "CastIceBolt":
                        command = new IceBoltCommand();
                        break;
                    case "RestartGame":
                        command = new RestartGameCommand();
                        break;
                    case "ExitGame":
                        command = new ExitGameCommand();
                        break;
                    
                }
                if(command != null)
                {
                    if(command is ICommand)
                    {
                        Commands.Push((ICommand)command);
                    }
                    command.Execute(MoveCommandTarget);
                }
            }
        }
    }
}
