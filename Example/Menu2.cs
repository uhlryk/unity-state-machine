using UnityEngine;
using System.Collections;
using Artwave.GameState;
public class Menu2 : MonoBehaviour {
	void Start(){

	}
	void OnGUI(){
		if (GUI.Button (new Rect ((Screen.width - 100) / 2, (Screen.height - 100) / 2, 100, 100), "Goto W")) {
			StateManager.SendEvent ("GotoWButtonClick");
		}
	}
}
