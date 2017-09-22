using System;
using UnityEditor;
using UnityEngine;

namespace SimonComponent.ActionScripts
{
    public class EnableLightDAS : AbstractDAS
    {
        public override string GetActionScriptName()
        {
            return "Enable Light";
        }

        public override void OnTurnOn(SimonDisplayer displayer)
        {
            Light light = displayer.GetComponent<Light>();
            if (light != null) light.enabled = true;
        }

        public override void OnTurnOff(SimonDisplayer displayer)
        {
            Light light = displayer.GetComponent<Light>();
            if (light != null) light.enabled = false;
        }
    }

    [CustomEditor(typeof(EnableLightDAS))]
    public class EnableLightDASEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EnableLightDAS scriptableObject = (EnableLightDAS) target;

            EditorGUILayout.HelpBox("Enable/Disable the light component", MessageType.None);
        }
    }
}