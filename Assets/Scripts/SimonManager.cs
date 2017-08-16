using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SimonInput
{
	Up,
	Left,
	Right,
	Down
}

public enum SimonStatus
{
	Initializing,
	Playing,
	Listening,
	Incrementializing,
	Celebrating
}

public class SimonManager : Singleton<SimonManager>
{
	// Graphics
	[SerializeField] private SimonDisplayer[] _displayers = new SimonDisplayer[4];
	// Logic
	[SerializeField] private SimonStatus _status = SimonStatus.Initializing;
	[SerializeField] private float _speed = 1f;
	[SerializeField] private List<int> _inputSerie = new List<int>();
	[SerializeField] private int _currentInputWaited = 0;
	[SerializeField] private int _pressedInput = -1;
	// Events
	public static event Action OnPlayingSerieStart;
	public static event Action OnPlayingSerieEnd;
	public static event Action OnListenInputStart;
	public static event Action OnListenInputStep;
	public static event Action OnListenInputEndGreat;
	public static event Action OnListenInputEndWrong;
	public static event Action OnCelebratingEnd;

	protected SimonManager() { } // guarantee this will be always a singleton only - can't use the constructor!
	
	#region Properties

	public SimonStatus Status
	{
		get { return _status; }
	}

	public float Speed
	{
		get { return _speed; }
	}

	public int CurrentInputWaited
	{
		get { return _currentInputWaited; }
	}

	public int InputSerieCount
	{
		get { return _inputSerie.Count; }
	}

	#endregion
	#region MonoBehaviour

	void Start ()
	{
		// Debug
		for (int i = 0; i < -999; i++)
		{
			AddInputInSerie();
		}
		
		OnPlayingSerieEnd += () => StartCoroutine(StartListenInput());
		OnListenInputEndGreat += EndTurn;
		OnListenInputEndWrong += () => Debug.Log("<b>WRONG !!!</b>");
		AddInputInSerie();
		StartCoroutine(PlaySerie());
	}
	
	void Update ()
	{
		if (_status == SimonStatus.Listening)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				HandleInput(SimonInput.Up);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				HandleInput(SimonInput.Left);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				HandleInput(SimonInput.Right);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				HandleInput(SimonInput.Down);
			}
		}
	}

	#endregion
	#region Simon Logic

	int AddInputInSerie()
	{
		// Generate a new value
		int newInput = Random.Range((int) SimonInput.Up, (int) SimonInput.Down + 1);
		// Add it in the serie
		_inputSerie.Add(newInput);
		// Return the value just added
		return newInput;
	}

	public void HandleInput(SimonInput input)
	{
		// In case of input pressed before treatment, do nothing
		if (_pressedInput >= 0) return;
		// Else handle it
		TurnAllDisplayersOff();
		int inputInt = (int) input;
		if (_displayers[inputInt] != null) _displayers[inputInt].TurnOn(_speed * .9f);
		_pressedInput = inputInt;
	}

	void ClearInputSerie()
	{
		_inputSerie.Clear();
	}

	void TurnAllDisplayersOff()
	{
		foreach (SimonDisplayer displayer in _displayers)
		{
			displayer.TurnOff();
		}
	}

	IEnumerator PlaySerie(float delay = 0f)
	{
		_status = SimonStatus.Playing;
		if (OnPlayingSerieStart != null) OnPlayingSerieStart();
		
		if (delay > 0) yield return new WaitForSeconds(delay);
		
		// For each input in serie
		foreach (int input in _inputSerie)
		{
			if (_displayers[input] != null)
			{
				_displayers[input].TurnOn(_speed * 0.9f);
			}
			yield return new WaitForSeconds(_speed);
		}
		if (OnPlayingSerieEnd != null) OnPlayingSerieEnd();
	}

	IEnumerator StartListenInput()
	{
		_status = SimonStatus.Listening;
		_currentInputWaited = 0;
		if (OnListenInputStart != null) OnListenInputStart();
		while (true)
		{
			yield return new WaitUntil(() => _pressedInput >= 0);
			// If the input is correct
			if (_pressedInput == _inputSerie[_currentInputWaited])
			{
				_pressedInput = -1;
				// If it's not the last
				if (_currentInputWaited < _inputSerie.Count - 1)
				{
					_currentInputWaited++;
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
				_status = SimonStatus.Celebrating;
				break;
			}
		}
	}

	protected void EndTurn()
	{
		AddInputInSerie();
		StartCoroutine(PlaySerie(_speed * 3f));
	}

	#endregion
}
