using System;
using UnityEditor;
using UnityEngine;

namespace SimonComponent.ActionScripts
{
    public class SendMessageDAS : AbstractDAS
    {
        public enum Types
        {
            Bool,
            Int,
            Float,
            Double,
            String,
            // UnityObject
        }

        public class SendMessageConfig
        {
            public string methodName = null;
            public Types parameterType = Types.String;
            public object parameter = null;
            public SendMessageOptions option = SendMessageOptions.RequireReceiver;
        }

        public SendMessageConfig turnOn = new SendMessageConfig();
        public SendMessageConfig turnOff = new SendMessageConfig();
        
        public override string GetActionScriptName()
        {
            return "Send Message";
        }

        public override void OnTurnOn(SimonDisplayer displayer)
        {
            if (!string.IsNullOrEmpty(turnOn.methodName))
            {
                displayer.SendMessage(turnOn.methodName, turnOn.parameter, turnOn.option);
            }
        }

        public override void OnTurnOff(SimonDisplayer displayer)
        {
            if (!string.IsNullOrEmpty(turnOff.methodName))
            {
                displayer.SendMessage(turnOff.methodName, turnOff.parameter, turnOff.option);
            }
        }
    }

    [CustomEditor(typeof(SendMessageDAS))]
    public class SendMessageDASEditor : Editor
    {
        bool gEnabled;
        
        public override void OnInspectorGUI()
        {
            gEnabled = GUI.enabled;
            SendMessageDAS scriptableObject = (SendMessageDAS) target;

            GUILayout.Label("Turn on :", EditorStyles.boldLabel);
            DrawSendMessageConfig(ref scriptableObject.turnOn);
            
            GUILayout.Label("Turn off :", EditorStyles.boldLabel);
            DrawSendMessageConfig(ref scriptableObject.turnOff);
            
            
        }

        public void DrawSendMessageConfig(ref SendMessageDAS.SendMessageConfig config)
        {
            config.methodName = EditorGUILayout.DelayedTextField("Method name", config.methodName);
            GUI.enabled = !string.IsNullOrEmpty(config.methodName);

            config.parameterType =(SendMessageDAS.Types)EditorGUILayout.EnumPopup("Parameter type", config.parameterType);

            try
            {
                switch (config.parameterType)
                {
                    case SendMessageDAS.Types.Bool:
                        bool boolValue = config.parameter == null || (bool)config.parameter;
                        config.parameter = EditorGUILayout.Toggle("Parameter", boolValue);
                        break;
                    case SendMessageDAS.Types.Int:
                        int intValue = config.parameter == null ? 0 : (int)config.parameter;
                        config.parameter = EditorGUILayout.IntField("Parameter", intValue);
                        break;
                    case SendMessageDAS.Types.Float:
                        float floatValue = config.parameter == null ? 0 : (float)config.parameter;
                        config.parameter = EditorGUILayout.FloatField("Parameter", floatValue);
                        break;
                    case SendMessageDAS.Types.Double:
                        double doubleValue = config.parameter == null ? 0 : (double)config.parameter;
                        config.parameter = EditorGUILayout.DoubleField("Parameter", doubleValue);
                        break;
                    case SendMessageDAS.Types.String:
                        string stringValue = config.parameter == null ? "" : (string)config.parameter;
                        config.parameter = EditorGUILayout.TextField("Parameter", stringValue);
                        break;
                    // case SendMessageDAS.Types.UnityObject:
                    //    break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                switch (config.parameterType)
                {
                    case SendMessageDAS.Types.Bool:
                        config.parameter = true;
                        break;
                    case SendMessageDAS.Types.Int:
                        config.parameter = 0;
                        break;
                    case SendMessageDAS.Types.Float:
                        config.parameter = 0f;
                        break;
                    case SendMessageDAS.Types.Double:
                        config.parameter = 0d;
                        break;
                    case SendMessageDAS.Types.String:
                        config.parameter = "";
                        break;
                    // case SendMessageDAS.Types.UnityObject:
                    //    break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            config.option = (SendMessageOptions)EditorGUILayout.EnumPopup("Option", config.option);
            GUI.enabled = gEnabled;
        }
    }
}
