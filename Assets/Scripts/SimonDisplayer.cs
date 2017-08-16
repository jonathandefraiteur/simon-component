using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonDisplayer : MonoBehaviour
{
	private Coroutine _timedTurnOnCoroutine;
	
	void Start ()
	{
		Light light = GetComponent<Light>();
		Renderer renderer = GetComponent<Renderer>();
		if (light != null && renderer != null)
		{
			light.color = renderer.material.color;
		}
	}
	
	void Update () {
		
	}

	[ContextMenu("Turn ON")]
	public void TurnOn(float duration = float.PositiveInfinity, float delay = 0f)
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
			ChangeEmmisionColor(GetComponent<Renderer>().material.GetColor("_Color"));
			GetComponent<Light>().enabled = true;
			// If there is an audio source
			AudioSource audioSource = GetComponent<AudioSource>();
			if (audioSource != null) audioSource.Play();
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
		// Remove emissive color and light
		ChangeEmmisionColor(Color.black);
		GetComponent<Light>().enabled = false;
	}

	protected void ChangeEmmisionColor(Color emissionColor)
	{
		GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
	}
}
