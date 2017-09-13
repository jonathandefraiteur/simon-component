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
		
		public event SimonPlayerActionEvent OnPlaySequenceStart;
		public event SimonPlayerSymbolEvent OnPlaySequenceStep;
		public event SimonPlayerActionEvent OnPlaySequencePause;
		public event SimonPlayerActionEvent OnPlaySequenceResume;
		public event SimonPlayerActionEvent OnPlaySequenceEnd;
		public event SimonPlayerActionEvent OnPlaySequenceStop;
		
		public event SimonPlayerActionEvent OnListenInputStart;
		public event SimonPlayerSymbolEvent OnInputReceived;
		public event SimonPlayerActionEvent OnListenInputStep;
		public event SimonPlayerActionEvent OnListenInputEndGreat;
		public event SimonPlayerActionEvent OnListenInputEndWrong;
		
		// Coroutines
		private Coroutine _playSequenceCoroutine;
		private Coroutine _listenInputCoroutine;

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

		[ContextMenu("Add symbol in sequence")]
		string AddSymbolInSequence()
		{
			// Generate a new value, and add it
			return AddSymbolInSequence(UnityEngine.Random.Range(0, Symbols.Length));
		}
		string AddSymbolInSequence(int newSymbolIndex)
		{
			// Add it in the serie
			_sequence.Add(newSymbolIndex);
			// Return the value just added
			return Symbols[newSymbolIndex];
		}

		// Gameplay controls
		[ContextMenu("Play sequence")]
		void PlaySequence()
		{
			PlaySequence(0f);
		}
		void PlaySequence(float delay)
		{
			_playSequenceCoroutine = StartCoroutine(PlaySequencCoroutine(delay));
		}
		[ContextMenu("Pause sequence")]
		void PauseSequence() {}
		[ContextMenu("Pause sequence")]
		void ResumeSequence() {}
		[ContextMenu("Stop sequence")]
		void StopSequence() {}
		
		void StartListenInput() {}
		void PauseListenInput() {}
		void ResumeListenInput() {}
		void StopListenInput() {}
		void SendInput(int input) {}
		void SendInput(string input) {}
		
		// Coroutines
		protected IEnumerator PlaySequencCoroutine(float delay = 0f)
		{
			// _status = SimonStatus.Playing;
			if (OnPlaySequenceStart != null) OnPlaySequenceStart();
		
			if (delay > 0) yield return new WaitForSeconds(delay);
		
			// For each input in serie
			foreach (int symbolIndex in _sequence)
			{
				if (OnPlaySequenceStep != null) OnPlaySequenceStep(Symbols[symbolIndex]);
				yield return new WaitForSeconds(_speed);
			}
			if (OnPlaySequenceEnd != null) OnPlaySequenceEnd();
		}
		
		
		#endregion
		#region Simon Logic (Protected)
		
		
		#endregion
	}
}
