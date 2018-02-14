//YIGIT OZTURK

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PedestrianControl))]
public class PeopleControlSetButton : Editor
{



	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();


		PedestrianControl myScript = (PedestrianControl)target;




		if (!myScript.isDone) {
			if(myScript.pressCount == 0)
			{
				if (GUILayout.Button ("Set Initial Position")) {
					myScript.SetInitialPos ();
					myScript.pressCount += 1;
				}
			}


			if(myScript.pressCount >= 1)
			{
				if (GUILayout.Button ("Set Position " + "(" + myScript.pressCount.ToString() + ")")) {

					myScript.SetPosition ();
					myScript.pressCount += 1;

				}

				if (GUILayout.Button ("Reset")) {
					myScript.ResetPositions ();
					myScript.pressCount = 0;
					myScript.isDone = false;
					EditorUtility.SetDirty(myScript);
				}
			}





			if(myScript.pressCount >= 2)
			{
				if (GUILayout.Button ("Done")) {
					myScript.DonePositioning ();

					myScript.isDone = true;
					EditorUtility.SetDirty(myScript);
				}	
			}


		} else {
			if (GUILayout.Button ("Reset Position")) {
				myScript.ResetPositions ();
				myScript.pressCount = 0;
				myScript.isDone = false;
				EditorUtility.SetDirty(myScript);
			}
		}


	}
}

