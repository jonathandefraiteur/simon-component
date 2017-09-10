using System;
using UnityEditor;
using UnityEngine;

namespace SimonComponent.ActionScripts
{
    public class PlayAudioSourceDAS : AbstractDAS
    {
        public bool StopOnTurnOff = false;
        
        public override string GetActionScriptName()
        {
            return "Play audio source";
        }

        public override void OnTurnOn(SimonDisplayer displayer)
        {
            // If there is an audio source
            AudioSource audioSource = displayer.GetComponent<AudioSource>();
            if (audioSource != null) audioSource.Play();
        }

        public override void OnTurnOff(SimonDisplayer displayer)
        {
            if (!StopOnTurnOff) return;
            
            // If there is an audio source
            AudioSource audioSource = displayer.GetComponent<AudioSource>();
            if (audioSource != null) audioSource.Stop();
        }
    }

    [CustomEditor(typeof(PlayAudioSourceDAS))]
    public class PlayAudioSourceDASEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PlayAudioSourceDAS scriptableObject = (PlayAudioSourceDAS) target;

            EditorGUILayout.HelpBox("Call Play() of the audio source component", MessageType.None);
            
            // Use material color
            scriptableObject.StopOnTurnOff =
                EditorGUILayout.Toggle("Stop on turn off", scriptableObject.StopOnTurnOff);
        }
    }
}