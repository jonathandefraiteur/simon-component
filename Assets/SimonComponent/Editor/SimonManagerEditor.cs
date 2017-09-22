using UnityEngine;
using UnityEditor;

namespace SimonComponent
{
    [CustomEditor(typeof(SimonManager))]
    // [CanEditMultipleObjects]
    public class SimonManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SimonManager script = (SimonManager) target;
            
            // SimonPLayer component is missing
            if (script.GetComponent<SimonPlayer>() == null)
            {
                EditorGUILayout.HelpBox("Require a SimonPlayer componnent on the same object.", MessageType.Error, true);
            }
            // Else if good, 
            else
            {
                EditorGUILayout.HelpBox("This manager allow to access the SimonPlayer component as Singleton.", MessageType.Info, true);
            }
        }
    }
}