﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityCommand
{
    public interface ICommandComponent
    {
        void Moving();
        void OnlyAim();
        void CastArcaneBolt();
        void CastFireBolt();
        void CastIceBolt();

    }
    public class CommandComponent : ICommandComponent
    {
        public void CastArcaneBolt()
        {
            throw new System.NotImplementedException();
        }

        public void CastFireBolt()
        {
            throw new System.NotImplementedException();
        }

        public void CastIceBolt()
        {
            throw new System.NotImplementedException();
        }

        public void Moving()
        {
            throw new System.NotImplementedException();
        }

        public void OnlyAim()
        {
            throw new System.NotImplementedException();
        }
    }

}