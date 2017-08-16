using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimonComponent
{
	public class SimonPlayer : MonoBehaviour
	{
		[SerializeField]
		private List<string> _states = new List<string>();
		public string[] States { get { return _states.ToArray(); }}
		
		// Events
		public static event Action OnPlayingSerieStart;
		public static event Action OnPlayingSerieEnd;
		
		public static event Action OnListenInputStart;
		public static event Action OnListenInputStep;
		public static event Action OnListenInputEndGreat;
		public static event Action OnListenInputEndWrong;
		
		public static event Action OnCelebratingEnd;

		private void Start () {
			
		}
		
		private void Update () {
			
		}

		public int GetStateIndex(string state)
		{
			// Look for the state
			for (int i = 0, c = _states.Count; i < c; i++)
			{
				// Test the match in lowercase
				if (state.ToLower() == _states[i].ToLower())
					return i;
			}
			// State not found
			return -1;
		}
	}
}
