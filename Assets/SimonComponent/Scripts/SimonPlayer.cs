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
		public enum Status
		{
			Idle,
			// Initializing,
			Playing,
			Listening,
			// Incrementializing,
			// Celebrating
		}
		
		// Configuration
		[SerializeField] private List<string> _symbols = new List<string>();
		public string[] Symbols { get { return _symbols.ToArray(); }}
		[SerializeField] private float _speed = 1f;
		public float Speed {get { return _speed; } set { _speed = value > 0f ? value : 0f; }}
		
		// Logic
		[SerializeField] private Status _status = Status.Playing;
		[SerializeField] private List<int> _sequence = new List<int>();
		public int[] Sequence { get { return _sequence.ToArray(); }}
		[SerializeField] private int _positionInSequence = -1;
		public int PositionInSequence { get { return _positionInSequence; }}
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
		private bool _playSequenceIsPaused;
		private Coroutine _listenInputCoroutine;
		private bool _listenInputIsPaused;

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

		[ContextMenu("Add symbol in sequence", false, 1100)]
		public string AddSymbolInSequence()
		{
			// Generate a new value, and add it
			return AddSymbolInSequence(UnityEngine.Random.Range(0, Symbols.Length));
		}
		
		public string AddSymbolInSequence(int newSymbolIndex)
		{
			// Add it in the serie
			_sequence.Add(newSymbolIndex);
			// Return the value just added
			return Symbols[newSymbolIndex];
		}

		[ContextMenu("Clear sequence", false, 1101)]
		public void ClearSymbolSequence()
		{
			if (_status == Status.Playing || _status == Status.Listening)
			{
				Debug.LogWarning("Can't clear the sequence during playing or listening !");
				return;
			}
				
			_sequence.Clear();
		}

		// Gameplay controls
		[ContextMenu("Play sequence", false, 1200)]
		public void PlaySequence()
		{
			PlaySequence(0f);
		}
		
		public void PlaySequence(float delay)
		{
			_playSequenceCoroutine = StartCoroutine(PlaySequenceCoroutine(delay));
		}
		
		[ContextMenu("Pause sequence", false, 1201)]
		public void PauseSequence()
		{
			_playSequenceIsPaused = true;
		}
		
		[ContextMenu("Resume sequence", false, 1203)]
		public void ResumeSequence()
		{
			_playSequenceIsPaused = false;
		}

		[ContextMenu("Stop sequence", false, 1204)]
		public void StopSequence()
		{
			throw new NotImplementedException();
		}

		[ContextMenu("Start listen", false, 1301)]
		public void StartListenInput()
		{
			StartListenInput(0f);
		}
		
		public void StartListenInput(float delay)
		{
			_listenInputCoroutine = StartCoroutine(ListenInputCoroutine(delay));
		}

		[ContextMenu("Pause listen", false, 1302)]
		public void PauseListenInput()
		{
			_listenInputIsPaused = true;
		}

		[ContextMenu("Resume listen", false, 1303)]
		public void ResumeListenInput()
		{
			_listenInputIsPaused = false;
		}

		[ContextMenu("Stop listen", false, 1304)]
		public void StopListenInput()
		{
			throw new NotImplementedException();
		}
		
		public void SendInput(int symbolIndex)
		{
			// In case of input pressed before treatment, do nothing
			if (_symbolSend >= 0) return;
			// Else handle it
			if (OnInputReceived != null) OnInputReceived(_symbols[symbolIndex]);
			_symbolSend = symbolIndex;
		}

		public void SendInput(string symbol)
		{
			SendInput(GetSymbolIndex(symbol));
		}
		
		#endregion
		#region Simon Logic (Protected)
		
		// Coroutines
		protected IEnumerator PlaySequenceCoroutine(float delay = 0f)
		{
			_status = Status.Playing;
		
			if (delay > 0) yield return new WaitForSeconds(delay);
			if (OnPlaySequenceStart != null) OnPlaySequenceStart();
		
			// Wait the end of frame to be synchronized in speed
			yield return new WaitForEndOfFrame();
			// For each symbol in serie (steps)
			foreach (int symbolIndex in _sequence)
			{
				// Handle pause
				if (_playSequenceIsPaused)
					yield return new WaitWhile((() => _playSequenceIsPaused));
				// Play event
				if (OnPlaySequenceStep != null)
					OnPlaySequenceStep(Symbols[symbolIndex]);
				// Wait before play another step
				yield return new WaitForSeconds(_speed);
			}
			if (OnPlaySequenceEnd != null) OnPlaySequenceEnd();
		}
		
		protected IEnumerator ListenInputCoroutine(float delay = 0f)
		{
			_status = Status.Listening;
			_positionInSequence = 0;
		
			if (delay > 0) yield return new WaitForSeconds(delay);
			if (OnListenInputStart != null) OnListenInputStart();
		
			while (true)
			{
				yield return new WaitUntil(() => _symbolSend >= 0);
				// If the input is correct
				if (_symbolSend == _sequence[_positionInSequence])
				{
					// Clear the symbol sended
					_symbolSend = -1;
					// If it's not the last
					if (_positionInSequence < _sequence.Count - 1)
					{
						_positionInSequence++;
						if (OnListenInputStep != null) OnListenInputStep();
					}
					else
					{
						if (OnListenInputEndGreat != null) OnListenInputEndGreat();
						break;
					}
				}
				// Wrong input
				else
				{
					if (OnListenInputEndWrong != null) OnListenInputEndWrong();
					// Error "Celebrating
					// _status = SimonStatus.Celebrating;
					break;
				}
			}
		}
		
		#endregion
	}
}
