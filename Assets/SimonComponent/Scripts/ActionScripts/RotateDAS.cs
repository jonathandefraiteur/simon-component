using UnityEditor;
using UnityEngine;

namespace SimonComponent.ActionScripts
{
	public class RotateDAS : AbstractDAS
	{
		public Vector3 rotation;
        
		public override string GetActionScriptName()
		{
			return "Rotation";
		}

		public override void OnTurnOn(SimonDisplayer displayer)
		{
			displayer.transform.Rotate(rotation);
		}

		public override void OnTurnOff(SimonDisplayer displayer)
		{
			displayer.transform.Rotate(-rotation);
		}
	}

	[CustomEditor(typeof(RotateDAS))]
	public class RotateDASEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			RotateDAS scriptableObject = (RotateDAS) target;

			scriptableObject.rotation = EditorGUILayout.Vector3Field("", scriptableObject.rotation);
		}
	}
}