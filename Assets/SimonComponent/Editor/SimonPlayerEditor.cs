using UnityEngine;
using UnityEditor;

namespace SimonComponent
{
	[CustomEditor(typeof(SimonPlayer))]
	// [CanEditMultipleObjects]
	public class SimonPlayerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			DrawDefaultInspector();
		}
	}
}
