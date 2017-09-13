using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimonComponent
{
	/// <summary>
	/// Ce composant a pour vacation d'avoir une sequence stockée, de la jouer, ou d'attendre des inputs.
	/// 
	/// Par jouer la séquence on entend que le player déclenche des évènements avec les données, mais il ne gère pas lui
	/// même les affichages ou animation résultant du jeu de la séquence.
	/// De la même façon il
	/// </summary>
	public class SimonPlayer : MonoBehaviour
	{
		// Configuration
		[SerializeField] private List<string> _symbols = new List<string>();
		public string[] Symbols { get { return _symbols.ToArray(); }}
		[SerializeField] private float _speed = 1f;
		
		// Logic
		[SerializeField] private List<int> _sequence = new List<int>();
		[SerializeField] private int _positionInSequence = -1;
		[SerializeField] private int _symbolSend = -1;
		
		// Events
		public delegate void SimonPlayerActionEvent();
		public delegate void SimonPlayerSymbolEvent(string symbol);
		
		public static event SimonPlayerActionEvent OnPlaySequenceStart;
		public static event SimonPlayerSymbolEvent OnPlaySequenceStep;
		public static event SimonPlayerActionEvent OnPlaySequencePause;
		public static event SimonPlayerActionEvent OnPlaySequenceResume;
		public static event SimonPlayerActionEvent OnPlaySequenceEnd;
		public static event SimonPlayerActionEvent OnPlaySequenceStop;
		
		public static event SimonPlayerActionEvent OnListenInputStart;
		public static event SimonPlayerSymbolEvent OnInputReceived;
		public static event SimonPlayerActionEvent OnListenInputStep;
		public static event SimonPlayerActionEvent OnListenInputEndGreat;
		public static event SimonPlayerActionEvent OnListenInputEndWrong;

		#region MonoBehaviour
		
		private void Start () {
			
		}
		
		private void Update () {
			
		}
		
		#endregion
		#region Simon Management

		public int GetSymbolIndex(string symbol)
		{
			// Look for the symbol
			for (int i = 0, c = _symbols.Count; i < c; i++)
			{
				// Test the match ingnoring case
				if (string.Equals(symbol, _symbols[i], StringComparison.OrdinalIgnoreCase))
					return i;
			}
			// Symbol not found
			return -1;
		}

		#endregion
		#region Simon Logic (Public)

		// Gameplay controls
		void PlaySequence() {}
		void PauseSequence() {}
		void StopSequence() {}
		
		void StartListenInput() {}
		void PauseListenInput() {}
		void StopListenInput() {}
		void SendInput(int input) {}
		void SendInput(string input) {}
		
		
		
		#endregion
		#region Simon Logic (Protected)
		
		
		#endregion
	}
}
