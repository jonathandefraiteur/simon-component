using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
		private string _listenedState;
		public string ListenedState
		{
			get { return _listenedState; }
			set
			{
				var index = SimonPlayer.GetStateIndex(value);
				// If the state doesn't not exist, do nothing
				if (index == -1) return;
				// Else, set the value
				_listenedState = value;
				_listenedStateIndex = index;
			}
		}
		private int _listenedStateIndex;
		public int ListenedStateIndex {get { return _listenedStateIndex; }}
		
		void Start ()
		{
			
		}
		
		void Update ()
		{
			
		}
	}
}
