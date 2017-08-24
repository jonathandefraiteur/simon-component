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
		
		void Start ()
		{
			
		}
		
		void Update ()
		{
			
		}
	}
}
