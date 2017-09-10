using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SimonComponent.ActionScripts;
using UnityEngine;

namespace SimonComponent
{
	public class SimonDisplayer : MonoBehaviour
	{
		[SerializeField]
		private SimonPlayer _sPlayer;
		public SimonPlayer SimonPlayer
		{
			get
			{
				// If there is a direct target set
				if (_sPlayer != null)
					return _sPlayer;
				// Else if there is SimonManager instancied
				else if (SimonManager.HasInstance())
					return SimonManager.Player;
				else
					return null;
			}
			set { _sPlayer = value; }
		}
		public bool ListenFromManager {get { return _sPlayer == null && SimonManager.HasInstance(); }}

		[SerializeField]
		private string _listenedSymbol;
		public string ListenedSymbol
		{
			get { return _listenedSymbol; }
			set
			{
				var index = SimonPlayer.GetSymbolIndex(value);
				// If the symbol doesn't not exist, do nothing
				if (index == -1) return;
				// Else, set the value
				_listenedSymbol = value;
				_listenedSymbolIndex = index;
			}
		}
		private int _listenedSymbolIndex;
		public int ListenedSymbolIndex {get { return _listenedSymbolIndex; }}

		[SerializeField]
		private bool _listenPlaying = true;
		public bool ListenPlaying {get { return _listenPlaying; } set { _listenPlaying = value; }}
		[SerializeField]
		private bool _listenSending = true;
		public bool ListenSending {get { return _listenSending; } set { _listenSending = value; }}

		public List<AbstractDAS> ActionScripts {get { return _actionScripts; }}

		[SerializeField]
		private List<AbstractDAS> _actionScripts = new List<AbstractDAS>();
		
		// Logic variables
		private Coroutine _timedTurnOnCoroutine;

		private delegate void SimonDisplayerEvent(SimonDisplayer displayer);
		private event SimonDisplayerEvent OnTurnOn;
		private event SimonDisplayerEvent OnTurnOff;

		#region MonoBehaviour

		protected void Start()
		{
			// If there is no SimonPlayer set to interact
			if (SimonPlayer == null)
			{
				Debug.LogWarning(this.name + " disabled 'cause no SimonPlayer set to interact", this);
				enabled = false;
				return;
			}
			
			LinkActionsScriptsToEvents();
		}
		
		void Update ()
		{
			
		}

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				LinkActionsScriptsToEvents();
			}
		}

		#endregion
		#region Displayer Logic

		protected void LinkActionsScriptsToEvents()
		{
			foreach (var actionScript in _actionScripts)
			{
				OnTurnOn += actionScript.OnTurnOn;
				OnTurnOff += actionScript.OnTurnOff;
			}
		}

		[ContextMenu("Turn ON")]
		public void TurnOn()
		{
			TurnOn(float.PositiveInfinity);
		}
		
		public void TurnOn(float duration, float delay = 0f)
		{
			// Stop actual coroutine if there is one
			if (_timedTurnOnCoroutine != null)
			{
				StopCoroutine(_timedTurnOnCoroutine);
				_timedTurnOnCoroutine = null;
			}
			// Simply turn on
			if (float.IsPositiveInfinity(duration))
			{
				if (OnTurnOn != null) OnTurnOn(this);
			}
			// Call TimedturnOn to turn on during a given time
			else
			{
				_timedTurnOnCoroutine = StartCoroutine(TimedTurnOn(duration, delay));
			}
		}

		protected IEnumerator TimedTurnOn(float duration, float delay = 0f)
		{
			// In case of delay...
			if (delay > 0) yield return new WaitForSeconds(delay);
			// Then turn on during the duration time, then turn off
			TurnOn();
			yield return new WaitForSeconds(duration);
			TurnOff();
		}

		[ContextMenu("Turn OFF")]
		public void TurnOff()
		{
			// Stop actual coroutine if there is one
			if (_timedTurnOnCoroutine != null)
			{
				StopCoroutine(_timedTurnOnCoroutine);
				_timedTurnOnCoroutine = null;
			}
			// Emit an event
			if (OnTurnOff != null) OnTurnOff(this);
		}

		#endregion
	}
}
