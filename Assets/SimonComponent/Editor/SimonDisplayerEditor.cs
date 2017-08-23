using UnityEngine;
using UnityEditor;

namespace SimonComponent
{
    [CustomEditor(typeof(SimonDisplayer))]
    // [CanEditMultipleObjects]
    public class SimonDisplayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SimonDisplayer script = (SimonDisplayer) target;

            // Simon Player
            script.SimonPlayer = (SimonPlayer) EditorGUILayout.ObjectField(new GUIContent("Simon Player"), (!script.ListenFromManager ? script.SimonPlayer : null), typeof(SimonPlayer), true);
            
            // If there is no Simon player linked
            if (script.SimonPlayer == null && FindObjectOfType<SimonManager>() == null)
            {
                EditorGUILayout.HelpBox("Require a SimonPlayer linked or a SimonManager in the scene.", MessageType.Warning, true);
            }
            // Else display more options
            else
            {
                // Due of the runing in Editor, the Instance is not necessarily set, so force it by the call of the property
                if (SimonManager.Instance == null) { return; }

                if (script.ListenFromManager)
                    EditorGUILayout.HelpBox("Listening from the SimonManager in the scene.", MessageType.None, true);
                
                EditorGUILayout.Separator();
                
                // State listened
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("State");
                int stateIndex = script.SimonPlayer.GetStateIndex(script.ListenedState);
                // State (in string) not refound
                if (stateIndex < 0)
                {
                    // Try to use the registered index
                    if (script.ListenedStateIndex > 0 && script.ListenedStateIndex < script.SimonPlayer.States.Length)
                        stateIndex = script.ListenedStateIndex;
                    // Else, by default the first state
                    else
                        stateIndex = 0;
                }
                script.ListenedState = script.SimonPlayer.States[EditorGUILayout.Popup(stateIndex, script.SimonPlayer.States)];
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
