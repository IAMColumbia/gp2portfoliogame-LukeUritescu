using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommand
{

    public class KeyMap
    {

        public Dictionary<KeyCode, string> OnReleasedKeyMap, OnKeyDownMap;

        public KeyMap()
        {
            OnReleasedKeyMap = new Dictionary<KeyCode, string>();
            OnKeyDownMap = new Dictionary<KeyCode, string>();
            this.Initialize();
        }

        public virtual void Initialize()
        {
            OnKeyDownMap.Add(KeyCode.Mouse0, "Moving");
            OnKeyDownMap.Add(KeyCode.Space, "OnlyAim");
            OnKeyDownMap.Add(KeyCode.Q, "CastArcaneBolt");
            OnKeyDownMap.Add(KeyCode.W, "CastFireBolt");
            OnKeyDownMap.Add(KeyCode.E, "CastIceBolt");
            OnKeyDownMap.Add(KeyCode.Escape, "ExitGame");
            OnKeyDownMap.Add(KeyCode.P, "RestartGame");
        }


    }

}
