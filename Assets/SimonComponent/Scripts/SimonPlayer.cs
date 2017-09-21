using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
		public enum PlayerMode
		{
			Player,
			Listener
		}
		
		public enum Status
		{
			Off,
			Running,
			Paused
		}
		
		// Configuration
		[SerializeField] private List<string> _symbols = new List<string>();
		public string[] Symbols { get { return _symbols.ToArray(); }}
		[SerializeField] private float _speed = 1f;
		public float Speed {get { return _speed; } set { _speed = value > 0f ? value : 0f; }}
		[SerializeField] private bool _lockTheRunningMode = false;
		public bool LockTheRunningMode
		{
			get { return _lockTheRunningMode; }
			set
			{
				if (_lockTheRunningMode && value == false && _status != Status.Off)
					throw new Exception("LockTheRunningMode security only can be disable when the player is Off.");
				_lockTheRunningMode = value;
			}
		}
		[SerializeField] private bool _lockTheRunningSequence = false;
		public bool LockTheRunningSequence
		{
			get { return _lockTheRunningSequence; }
			set
			{
				if (_lockTheRunningSequence && value == false && _status != Status.Off)
					throw new Exception("LockTheRunningSequence security only can be disable when the player is Off.");
				_lockTheRunningSequence = value;
			}
		}
		
		
		// Logic
		[SerializeField] private PlayerMode _mode = PlayerMode.Player;
		public PlayerMode Mode
		{
			get { return _mode; }
			set
			{
				if (_lockTheRunningMode && _status != Status.Off && _mode != value)
					throw new Exception("Unable to change the mode because of the LockTheRunningMode security.");
				if (_status != Status.Off)
					Stop();
				_mode = value;
			}
		}
		[SerializeField] private Status _status = Status.Off;
		[SerializeField] private List<int> _sequence = new List<int>();
		public int[] Sequence { get { return _sequence.ToArray(); }}
		[SerializeField] private int _positionInSequence = -1;
		public int PositionInSequence { get { return _positionInSequence; }}
		[SerializeField] private int _symbolSend = -1;
		
		// Events
		public delegate void SimonPlayerActionEvent();
		public delegate void SimonPlayerSymbolEvent(string symbol);
		
		public event SimonPlayerActionEvent OnPlayingSequenceStart;
		public event SimonPlayerSymbolEvent OnPlayingSequenceStep;
		public event SimonPlayerActionEvent OnPlayingSequencePause;
		public event SimonPlayerActionEvent OnPlayingSequenceResume;
		public event SimonPlayerActionEvent OnPlayingSequenceEnd;
		public event SimonPlayerActionEvent OnPlayingSequenceStop;
		
		public event SimonPlayerActionEvent OnListenInputStart;
		public event SimonPlayerSymbolEvent OnInputReceived;
		public event SimonPlayerActionEvent OnListenInputStep;
		public event SimonPlayerActionEvent OnListenInputEndGreat;
		public event SimonPlayerActionEvent OnListenInputEndWrong;
		
		// Coroutines
		private Coroutine _currentCoroutine;

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

		public string AddSymbolInSequence()
		{
			// Generate a new value, and add it
			return AddSymbolInSequence(UnityEngine.Random.Range(0, Symbols.Length));
		}
		
		public string AddSymbolInSequence(int newSymbolIndex)
		{
			// Check security
			if (_lockTheRunningSequence && _status != Status.Off)
				throw new Exception("Unable to edit the sequence when it's played (even if paused) because of the LockTheRunningSequence security.");
			// Add it in the serie
			_sequence.Add(newSymbolIndex);
			// Return the value just added
			return Symbols[newSymbolIndex];
		}

		public void ClearSymbolSequence()
		{
			// Check security
			if (_lockTheRunningSequence && _status != Status.Off)
				throw new Exception("Unable to clear the sequence when it's played (even paused) because of the LockTheRunningSequence security.");
			_sequence.Clear();
		}

		// Gameplay controls
		public void PlaySequence(float delay = 0f)
		{
			Mode = PlayerMode.Player;
			Run(delay);
		}
		
		public void StartListenInput(float delay = 0f)
		{
			Mode = PlayerMode.Listener;
			Run(delay);
		}

		public void Run(float delay = 0f)
		{
			// TODO:  Think about the most logic, restart or forbid the new run
			if (_status != Status.Off)
				Stop();
			switch (_mode)
			{
				case PlayerMode.Player:
					_currentCoroutine = StartCoroutine(PlaySequenceCoroutine(delay));
					break;
				case PlayerMode.Listener:
					_currentCoroutine = StartCoroutine(ListenInputCoroutine(delay));
					break;
				default:
					throw new InvalidEnumArgumentException("Unknown mode. ("+ (int)_mode +")");
			}
		}
		
		public void Pause()
		{
			if (_status != Status.Running) return;
			_status = Status.Paused;
		}
		
		public void Resume()
		{
			if (_status != Status.Paused) return;
			_status = Status.Running;
		}

		public void Stop()
		{
			if (_status == Status.Off) return;
			_status = Status.Off;
			_positionInSequence = -1;
			_symbolSend = -1;
		}
		
		public void SendInput(int symbolIndex)
		{
			if (_mode != PlayerMode.Listener)
			{
				Debug.LogWarning("Trying to send input out of Listener mode");
				return;
			}
			if (_status != Status.Running)
			{
				Debug.Log("Input ignored because the player is not running");
				return;
			}
			
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
			_mode = PlayerMode.Player;
			_status = Status.Running;
			_positionInSequence = 0;
		
			if (delay > 0) yield return new WaitForSeconds(delay);
			if (OnPlayingSequenceStart != null) OnPlayingSequenceStart();
		
			// Wait the end of frame to be synchronized in speed
			yield return new WaitForEndOfFrame();
			// For each symbol in serie (steps)
			foreach (int symbolIndex in _sequence)
			{
				// Handle pause
				if (_status == Status.Paused)
					yield return new WaitWhile((() => _status == Status.Paused));
				// Play event
				_positionInSequence++;
				if (OnPlayingSequenceStep != null)
					OnPlayingSequenceStep(Symbols[symbolIndex]);
				// Wait before play another step
				yield return new WaitForSeconds(_speed);
			}
			_status = Status.Off;
			if (OnPlayingSequenceEnd != null) OnPlayingSequenceEnd();
		}
		
		protected IEnumerator ListenInputCoroutine(float delay = 0f)
		{
			_mode = PlayerMode.Listener;
			_status = Status.Running;
			_positionInSequence = 0;
		
			if (delay > 0) yield return new WaitForSeconds(delay);
			if (OnListenInputStart != null) OnListenInputStart();
		
			while (true)
			{
				// Handle pause
				if (_status == Status.Paused)
					yield return new WaitWhile((() => _status == Status.Paused));
				// Wait the reception of a symbol
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
						_status = Status.Off;
						if (OnListenInputEndGreat != null) OnListenInputEndGreat();
						break;
					}
				}
				// Wrong input
				else
				{
					_status = Status.Off;
					if (OnListenInputEndWrong != null) OnListenInputEndWrong();
					// Error "Celebrating
					// _mode = SimonStatus.Celebrating;
					break;
				}
			}
		}
		
		#endregion
	}
}
