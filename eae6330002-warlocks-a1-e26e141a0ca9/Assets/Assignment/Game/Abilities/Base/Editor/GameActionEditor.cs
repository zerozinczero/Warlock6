using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TurnToDirectionAction))]
public class GameActionEditor : Editor {

    public override void OnInspectorGUI() {
        GameAction action = target as GameAction;

        base.OnInspectorGUI();

		if(GUILayout.Button("Perform")) {
			action.Perform();
		}

        if(GUILayout.Button("Stop")) {
			action.Stop();
		}
    }

}