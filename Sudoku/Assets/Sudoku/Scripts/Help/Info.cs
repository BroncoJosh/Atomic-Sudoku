using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Info : MonoBehaviour {
    string clueWeight1 = "2", clueWeight2 = "4", max1 = "3";

	void OnGUI () {
        GUI.Box(new Rect(200, 2, Screen.width-204, 80), "Left mouse button - increase value in non-clue field.\nRight mouse button - decrease value in non-clue field.\nScroll wheel - delete value from non-clue field\nCurrent Android version support only left mouse button functionality.");
        clueWeight1 = GUI.TextField(new Rect(170, 5, 20, 20), clueWeight1);
        clueWeight2 = GUI.TextField(new Rect(170, 30, 20, 20), clueWeight2);
        max1 = GUI.TextField(new Rect(170, 55, 20, 20), max1);

        GUI.Box(new Rect(2, 5, 168, 22), "Clue weight 1");
        GUI.Box(new Rect(2, 30, 168, 22), "Clue weight 2");
        GUI.Box(new Rect(2, 55, 168, 22), "Max. clues in region");

        if (GUI.Button(new Rect(2, 105, 68, 22), "Generate")) {
            SudokuGenerator.Instance.clueWeight1 = int.Parse(clueWeight1);
            SudokuGenerator.Instance.clueWeight2 = int.Parse(clueWeight2);
            SudokuGenerator.Instance.maxCluesInRegion = int.Parse(max1);
            SudokuGenerator.Instance.Next();
        }

        GUI.Box(new Rect(2, 80, 168, 22), "Repeated: " + SudokuGenerator.Instance.repeats.ToString() + "/" + SudokuGenerator.Instance.generates.Count.ToString());
	}
}
