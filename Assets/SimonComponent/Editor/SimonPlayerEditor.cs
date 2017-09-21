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
			
			// Player
			GUILayout.Label("Player", EditorStyles.boldLabel);
			DrawPlayer(ref script);
			
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
				if (i % 10 != 0) continue;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
			}
			
			EditorGUILayout.EndHorizontal();



			if (GUILayout.Button("Random"))
			{
				script.AddSymbolInSequence();
			}
		}

		protected void DrawPlayer(ref SimonPlayer script)
		{
			bool gEnabled = GUI.enabled;
			bool playerEnabled = gEnabled && Application.isPlaying;
			GUI.enabled = playerEnabled;

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			if (GUILayout.Button("►", EditorStyles.miniButtonLeft, GUILayout.Width(30), GUILayout.Height(20)))
				script.Run();
			GUI.enabled = playerEnabled && script.Status != SimonPlayer.PlayerStatus.Off;
			bool paused = GUILayout.Toggle(script.Status == SimonPlayer.PlayerStatus.Paused, "▌▐", EditorStyles.miniButtonMid, GUILayout.Width(30), GUILayout.Height(20));
			if (GUI.enabled)
			{
				if (paused && script.Status == SimonPlayer.PlayerStatus.Running)
					script.Pause();
				else if (!paused && script.Status == SimonPlayer.PlayerStatus.Paused)
					script.Resume();
			}
			GUI.enabled = playerEnabled && script.Status != SimonPlayer.PlayerStatus.Off;
			if (GUILayout.Button("██", EditorStyles.miniButtonRight, GUILayout.Width(30), GUILayout.Height(20)))
				script.Stop();
			GUI.enabled = playerEnabled;
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			GUI.enabled = gEnabled;
		}
	}
}
