using System;
using UnityEngine;

namespace SimonComponent
{
    public interface IDisplayerActionScript
    {
        string GetActionScriptName();
        void OnTurnOn(SimonDisplayer displayer);
        void OnTurnOff(SimonDisplayer displayer);
    }
}
