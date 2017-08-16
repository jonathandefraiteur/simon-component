using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
	[SerializeField] private SimonInput _input = SimonInput.Up;
	
	private void OnMouseDown()
	{
		if (SimonManager.Instance != null)
		{
			SimonManager.Instance.HandleInput(_input);
		}
	}
}
