using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonCamera : MonoBehaviour
{
	[SerializeField] private Vector3[] _positions = new Vector3[1];
	[SerializeField] private float _speed = .25f;
	private Coroutine _moveToCoroutine;
	
	void Start () {
		
	}
	
	void Update () {
		
	}

	public void MoveTo(int position)
	{
		if (_moveToCoroutine != null)
		{
			StopCoroutine(_moveToCoroutine);
		}
		StartCoroutine(MoveToCoroutine(position % _positions.Length, _speed));
	}

	protected IEnumerator MoveToCoroutine(int position, float speed)
	{
		while (Vector3.Distance(_positions[position], transform.position) > .05f)
		{
			transform.position = Vector3.Lerp(transform.position, _positions[position], speed);
			yield return null;
		}
		transform.position = _positions[position];
		_moveToCoroutine = null;
	}

	[ContextMenu("Move to Next")]
	public void MoveToNextPosition()
	{
		for (int i = 0; i < _positions.Length; i++)
		{
			if (_positions[i] == transform.position)
			{
				MoveTo(i+1);
				return;
			}
		}
		MoveTo(0);
	}

	[ContextMenu("Move to Previous")]
	public void MoveToPreviousPosition()
	{
		for (int i = _positions.Length -1; i >= 0; i--)
		{
			if (_positions[i] == transform.position)
			{
				MoveTo(i-1);
				return;
			}
		}
		MoveTo(0);
	}
}
