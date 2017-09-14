using UnityEngine;
using UnityEditor;

namespace SimonComponent
{
	[CustomEditor(typeof(SimonPlayer))]
	// [CanEditMultipleObjects]
	public class SimonPlayerEditor : Editor
	{
		private Color _guiOriginalBackgroundColor;
		
		public override void OnInspectorGUI()
		{
			_guiOriginalBackgroundColor = GUI.backgroundColor;
			SimonPlayer script = (SimonPlayer) target;
			
			
			
			EditorGUILayout.Space();
			GUI.backgroundColor = _guiOriginalBackgroundColor;
			
			// Sequence
			GUILayout.Label("Sequence", EditorStyles.boldLabel);
			DrawSequence(ref script);
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			DrawDefaultInspector();
		}

		protected void DrawSequence(ref SimonPlayer script)
		{
			GUILayout.BeginHorizontal();

			int i = 0;
			foreach (int symbolIndex in script.Sequence)
			{
				if (i < script.PositionInSequence)
					GUI.backgroundColor = Color.grey;
				else if (i == script.PositionInSequence)
					GUI.backgroundColor = Color.cyan;
					
				GUILayout.Box(symbolIndex.ToString(), EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(18));

				GUI.backgroundColor = _guiOriginalBackgroundColor;


				i++;
				if (i % 10 == 0)
				{
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
				}
			}
			
			EditorGUILayout.EndHorizontal();



			if (GUILayout.Button("Random"))
			{
				script.AddSymbolInSequence();
			}
		}
	}
}
