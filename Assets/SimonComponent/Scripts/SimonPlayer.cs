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
		public static event Action OnPlaySequenceStart;
		public static event Action OnPlaySequenceStep;
		public static event Action OnPlaySequencePause;
		public static event Action OnPlaySequenceResume;
		public static event Action OnPlaySequenceEnd;
		public static event Action OnPlaySequenceStop;
		
		public static event Action OnListenInputStart;
		public static event Action OnInputReceived;
		public static event Action OnListenInputStep;
		public static event Action OnListenInputEndGreat;
		public static event Action OnListenInputEndWrong;

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
