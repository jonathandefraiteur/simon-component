using System;
using UnityEditor;
using UnityEngine;

namespace SimonComponent.ActionScripts
{
    public class EmissionColorChangeDAS : AbstractDAS
    {
        public Color color;
        
        public override string GetActionScriptName()
        {
            return "Emission Color Change";
        }

        public override void OnTurnOn(SimonDisplayer displayer)
        {
            ChangeEmmisionColor(displayer, displayer.GetComponent<Renderer>().material.GetColor("_Color"));
        }

        public override void OnTurnOff(SimonDisplayer displayer)
        {
            ChangeEmmisionColor(displayer, Color.black);
        }
        
        protected void ChangeEmmisionColor(SimonDisplayer displayer, Color emissionColor)
        {
            displayer.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
        }
    }

    [CustomEditor(typeof(EmissionColorChangeDAS))]
    public class EmissionColorChangeDASEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EmissionColorChangeDAS scriptableObject = (EmissionColorChangeDAS) target;

            scriptableObject.color = EditorGUILayout.ColorField("Color", scriptableObject.color);
        }
    }
}