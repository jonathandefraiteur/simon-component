using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SimonScoreText : MonoBehaviour
{
	private void Start()
	{
		PrintScore();
		SimonManager.OnPlayingSerieStart += () => PrintScore(-1, SimonManager.Instance.InputSerieCount);
		SimonManager.OnListenInputStart += () => PrintScore(SimonManager.Instance.CurrentInputWaited, SimonManager.Instance.InputSerieCount);
		SimonManager.OnListenInputStep += () => PrintScore(SimonManager.Instance.CurrentInputWaited, SimonManager.Instance.InputSerieCount);
		SimonManager.OnListenInputEndGreat += () => PrintScore(-1, SimonManager.Instance.InputSerieCount);
	}

	private void PrintScore(int currentInput = -1, int lastInput = 0)
	{
		GetComponent<Text>().text = (currentInput < 0 ? "-" : (currentInput + 1) + "") + " / " + lastInput;
	}
}
