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
		public SimonPlayer SimonPlayer {get { return _sPlayer; } set { _sPlayer = value; }}

		[SerializeField]
		private string _listenedState;
		public string ListenedState
		{
			get { return _listenedState; }
			set
			{
				var index = _sPlayer.GetStateIndex(value);
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
