using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using SimonComponent.ActionScripts;
using UnityEngine;
using UnityEditor;

namespace SimonComponent
{
    [CustomEditor(typeof(SimonDisplayer))]
    // [CanEditMultipleObjects]
    public class SimonDisplayerEditor : Editor
    {
        protected enum DrawActionEvent
        {
            None,
            Remove,
            MoveUp,
            MoveDown
        }
        
        private int indexPopupAddDAS = 0;
        
        public override void OnInspectorGUI()
        {
            var guiOriginalBackgroundColor = GUI.backgroundColor;
            SimonDisplayer script = (SimonDisplayer) target;

            // Simon Player
            script.SimonPlayer = (SimonPlayer) EditorGUILayout.ObjectField(new GUIContent("Simon Player"), (!script.ListenFromManager ? script.SimonPlayer : null), typeof(SimonPlayer), true);

            // Due of the runing in Editor, the Instance is not necessarily set,
            // so force it by the call of the property if there is a component SimonManager in the scene
            if (FindObjectOfType<SimonManager>() != null)
            {
                if (SimonManager.Instance == null) { return; }
            }
            
            // If there is no Simon player linked
            if (script.SimonPlayer == null)
            {
                EditorGUILayout.HelpBox("Require a SimonPlayer linked or a SimonManager in the scene", MessageType.Warning, true);
            }
            // Else display more options
            else
            {
                if (script.ListenFromManager)
                    EditorGUILayout.HelpBox("Listening from the SimonManager in the scene", MessageType.None, true);
                
                // Symbol listened
                EditorGUILayout.BeginHorizontal();
                // If there is no symbol in the player, display a message
                if (script.SimonPlayer.Symbols.Length == 0)
                {
                    EditorGUILayout.HelpBox("No symbols set in the SimonPlayer listened\n\nYou must add at least one symbol to configure the displayer", MessageType.Warning, true);
                }
                // Else, draw a popup of symbols
                else
                {
                    EditorGUILayout.PrefixLabel("Listened symbol");
                    int symbolIndex = script.SimonPlayer.GetSymbolIndex(script.ListenedSymbol);
                    // Symbol (in string) not refound
                    if (symbolIndex < 0)
                    {
                        // Try to use the registered index
                        if (script.ListenedSymbolIndex > 0 && script.ListenedSymbolIndex < script.SimonPlayer.Symbols.Length)
                            symbolIndex = script.ListenedSymbolIndex;
                        // Else, by default the first symbol
                        else
                            symbolIndex = 0;
                    }
                    script.ListenedSymbol = script.SimonPlayer.Symbols[EditorGUILayout.Popup(symbolIndex, script.SimonPlayer.Symbols)];
                }
                EditorGUILayout.EndHorizontal();
                
                // Actions
                EditorGUILayout.Separator();
                
                GUILayout.Label("Actions scripts");
                
                // Actions : List
                for (int i = 0, count = script.ActionScripts.Count; i < count; i++)
                {
                    var actionScript = script.ActionScripts[i];
                    var actionEvent = DrawAction(actionScript);
                    if (actionEvent == DrawActionEvent.Remove)
                    {
                        script.ActionScripts.RemoveAt(i);
                    }
                    else if (actionEvent == DrawActionEvent.MoveUp && i > 0)
                    {
                        script.ActionScripts.RemoveAt(i);
                        script.ActionScripts.Insert(i - 1, actionScript);
                    }
                    else if (actionEvent == DrawActionEvent.MoveDown && i < count - 1)
                    {
                        script.ActionScripts.RemoveAt(i);
                        script.ActionScripts.Insert(i + 1, actionScript);
                    }
                    if (actionEvent != DrawActionEvent.None)
                    {
                        break;
                    }
                }

                // Actions : Add
                var types = AbstractDAS.GetDisplayerActionScripts();
                var typeNames = new List<string>();
                foreach (Type type in types)
                    typeNames.Add(type.Name.Replace("DAS", ""));

                EditorGUILayout.BeginHorizontal();
                indexPopupAddDAS = EditorGUILayout.Popup(indexPopupAddDAS, typeNames.ToArray());
                GUI.backgroundColor = new Color(.5f, .9f, .3f);
                if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.MaxWidth(23)))
                    script.ActionScripts.Add((AbstractDAS) ScriptableObject.CreateInstance(types[indexPopupAddDAS].Name));
                GUI.backgroundColor = guiOriginalBackgroundColor;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Separator();
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }

        private DrawActionEvent DrawAction(AbstractDAS actionScript)
        {
            var guiOriginalBackgroundColor = GUI.backgroundColor;
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // Title bar
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(actionScript.GetActionScriptName(), EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("▲", EditorStyles.miniButtonMid, GUILayout.MaxWidth(19))) return DrawActionEvent.MoveUp;
            if (GUILayout.Button("▼", EditorStyles.miniButtonMid, GUILayout.MaxWidth(19))) return DrawActionEvent.MoveDown;
            GUI.backgroundColor = new Color(1f, .4f, .4f);
            if (GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.MaxWidth(19))) return DrawActionEvent.Remove;
            GUI.backgroundColor = guiOriginalBackgroundColor;
            EditorGUILayout.EndHorizontal();
            
            // Body
            EditorGUILayout.Separator();
            CreateEditor(actionScript).OnInspectorGUI();

            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();

            return DrawActionEvent.None;
        }
    }
}
