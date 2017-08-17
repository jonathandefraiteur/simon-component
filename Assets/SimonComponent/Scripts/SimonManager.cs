using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

namespace SimonComponent
{
    [RequireComponent(typeof(SimonPlayer))]
    public class SimonManager : Singleton<SimonManager>
    {
        protected SimonManager() {} // guarantee this will be always a singleton only - can't use the constructor!

        public static SimonPlayer Player
        {
            get
            {
                var player = Instance.GetComponent<SimonPlayer>();
                if (player == null) throw new Exception("SimonManager object doesn't have SimonPlayer component on it.");
                return player;
            }
        }
    }
}
