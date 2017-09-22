using System;
using UnityEditor;
using UnityEngine;

namespace SimonComponent.ActionScripts
{
    public class EmissionColorChangeDAS : AbstractDAS
    {
        public bool UseMaterialColor = true;
        public Color Color = Color.white;
        
        public override string GetActionScriptName()
        {
            return "Emission Color Change";
        }

        public override void OnTurnOn(SimonDisplayer displayer)
        {
            if (UseMaterialColor)
            {
                Renderer renderer = displayer.GetComponent<Renderer>();
                if (renderer == null) return;
                Color = renderer.material.GetColor("_Color");
            }
            ChangeEmmisionColor(displayer, Color);
        }

        public override void OnTurnOff(SimonDisplayer displayer)
        {
            ChangeEmmisionColor(displayer, Color.black);
        }
        
        protected void ChangeEmmisionColor(SimonDisplayer displayer, Color emissionColor)
        {
            Renderer renderer = displayer.GetComponent<Renderer>();
            if (renderer != null) renderer.material.SetColor("_EmissionColor", emissionColor);
        }
    }

    [CustomEditor(typeof(EmissionColorChangeDAS))]
    public class EmissionColorChangeDASEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EmissionColorChangeDAS scriptableObject = (EmissionColorChangeDAS) target;

            // Use material color
            scriptableObject.UseMaterialColor =
                EditorGUILayout.Toggle(new GUIContent("Use material color", "Use property _Color of the main material"), scriptableObject.UseMaterialColor);
            // Color
            GUI.enabled = !scriptableObject.UseMaterialColor;
            scriptableObject.Color = EditorGUILayout.ColorField("Color", scriptableObject.Color);
            GUI.enabled = true;
        }
    }
}