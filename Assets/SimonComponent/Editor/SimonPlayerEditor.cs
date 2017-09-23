using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace SimonComponent
{
	[CustomEditor(typeof(SimonPlayer))]
	// [CanEditMultipleObjects]
	public class SimonPlayerEditor : Editor
	{
		private Color _guiOriginalBackgroundColor;
		private AnimBool _showDefaultInspector;
		
		void OnEnable()
		{
			_showDefaultInspector = new AnimBool(false);
			_showDefaultInspector.valueChanged.AddListener(Repaint);
		}
		
		public override void OnInspectorGUI()
		{
			_guiOriginalBackgroundColor = GUI.backgroundColor;
			SimonPlayer script = (SimonPlayer) target;
			
			EditorGUILayout.Space();
			GUI.backgroundColor = _guiOriginalBackgroundColor;

			// Player
			GUI.enabled = Application.isPlaying;
			GUILayout.Label("Player", EditorStyles.boldLabel);
			DrawPlayer(ref script);
			GUI.enabled = true;
			
			// Configuration
			GUILayout.Label("Configuration", EditorStyles.boldLabel);
			DrawConfiguration(ref script);
			
			// Sequence
			GUILayout.Label("Sequence", EditorStyles.boldLabel);
			DrawSequence(ref script);
			
			// Default inspector
			EditorGUILayout.Space();
			_showDefaultInspector.target = EditorGUILayout.Foldout(_showDefaultInspector.target, "Default Inspector");
			if (EditorGUILayout.BeginFadeGroup(_showDefaultInspector.faded))
			{
				DrawDefaultInspector();
			}
			EditorGUILayout.EndFadeGroup();
		}

		protected void DrawConfiguration(ref SimonPlayer script)
		{
			script.Mode = (SimonPlayer.PlayerMode)EditorGUILayout.EnumPopup("Mode", script.Mode);
			script.Speed = EditorGUILayout.FloatField("Interval (s)", script.Speed);
			GUILayout.Label("Lock during run :");
			GUILayout.BeginHorizontal();
			GUILayout.Space(10);
			script.LockTheRunningMode = EditorGUILayout.ToggleLeft("Mode", script.LockTheRunningMode, GUILayout.Width(50));
			script.LockTheRunningSequence = EditorGUILayout.ToggleLeft("Sequence", script.LockTheRunningSequence, GUILayout.Width(80));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		
		protected void DrawSequence(ref SimonPlayer script)
		{
			// Progress
			GUILayout.HorizontalSlider(script.PositionInSequence, 0, script.Sequence.Length - 1);
			
			// Sequence
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
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
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			



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

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(GUILayout.Width(140));

			// Mode
			GUI.enabled = playerEnabled && !script.LockTheRunningMode;
			GUILayout.BeginHorizontal();
			bool reachPlayerMode = GUILayout.Toggle(script.Mode == SimonPlayer.PlayerMode.Player, SimonPlayer.PlayerMode.Player.ToString(), EditorStyles.miniButtonLeft);
			if (reachPlayerMode && script.Mode != SimonPlayer.PlayerMode.Player)
			{
				script.Mode = SimonPlayer.PlayerMode.Player;
			}
			bool reachListenerMode = GUILayout.Toggle(script.Mode == SimonPlayer.PlayerMode.Listener, SimonPlayer.PlayerMode.Listener.ToString(), EditorStyles.miniButtonRight);
			if (reachListenerMode && script.Mode != SimonPlayer.PlayerMode.Listener)
			{
				script.Mode = SimonPlayer.PlayerMode.Listener;
			}
			GUILayout.EndHorizontal();
			GUI.enabled = playerEnabled;
			
			// Status
			const int buttonsHeight = 20;
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("►", EditorStyles.miniButtonLeft, GUILayout.Height(buttonsHeight)))
			{
				script.Run();
			}
			GUI.enabled = playerEnabled && script.Status != SimonPlayer.PlayerStatus.Off;
			bool paused = GUILayout.Toggle(script.Status == SimonPlayer.PlayerStatus.Paused, "▌▐", EditorStyles.miniButtonMid, GUILayout.Height(buttonsHeight));
			if (GUI.enabled)
			{
				if (paused && script.Status == SimonPlayer.PlayerStatus.Running)
					script.Pause();
				else if (!paused && script.Status == SimonPlayer.PlayerStatus.Paused)
					script.Resume();
			}
			GUI.enabled = playerEnabled && script.Status != SimonPlayer.PlayerStatus.Off;
			if (GUILayout.Button("██", EditorStyles.miniButtonRight, GUILayout.Height(buttonsHeight)))
			{
				script.Stop();
			}
			GUI.enabled = playerEnabled;
			GUILayout.EndHorizontal();
			
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			GUI.enabled = gEnabled;
		}
	}
}
