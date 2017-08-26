using UnityEditor;
using UnityEngine;

namespace SimonComponent.ActionScripts
{
    public class TranslateDAS : AbstractDAS
    {
        public Vector3 translation;
        
        public override string GetActionScriptName()
        {
            return "Translation";
        }

        public override void OnTurnOn(SimonDisplayer displayer)
        {
            displayer.transform.Translate(translation);
        }

        public override void OnTurnOff(SimonDisplayer displayer)
        {
            displayer.transform.Translate(-translation);
        }
    }

    [CustomEditor(typeof(TranslateDAS))]
    public class TranslateDASEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TranslateDAS scriptableObject = (TranslateDAS) target;

            scriptableObject.translation = EditorGUILayout.Vector3Field("", scriptableObject.translation);
        }
    }
}