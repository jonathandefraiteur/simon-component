using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SimonComponent.ActionScripts
{
    [Serializable]
    public abstract class AbstractDAS : ScriptableObject
    {
        public static List<Type> GetDisplayerActionScripts()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            List<Type> derivedTypes = new List<Type>();

            for (int i = 0, count = types.Length; i < count; i++)
            {
                Type type = types[i];
                if (IsDisplayerActionScript(type))
                {
                    derivedTypes.Add((type));
                }
            }

            return derivedTypes;
        }

        public static bool IsDisplayerActionScript(Type type)
        {
            Type baseType = typeof(AbstractDAS);

            if (type == null || type == baseType)
                return false;

            return type.IsSubclassOf(baseType) && !type.IsAbstract;
        }
        
        public abstract string GetActionScriptName();
        public abstract void OnTurnOn(SimonDisplayer displayer);
        public abstract void OnTurnOff(SimonDisplayer displayer);
    }
}
