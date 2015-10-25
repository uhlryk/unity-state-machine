using UnityEngine;
using System.Collections;
using Artwave.GameState;
public class StateY : AbstractState {
	private Menu2 menu;
	public override void OnActive(){
		Debug.Log ("Aktywacja StateY");
		menu = GameObject.Find ("GUI").GetComponent("Menu2") as Menu2;
		menu.enabled = true;
		Debug.Log (menu);
	}
	public override void OnReceiveEvent(string message){
		Debug.Log ("Event StateY " + message);
		
		if (message == "GotoWButtonClick") {
			StateManager.ChangeState ("SceneA","StateW.StateW");
		}
	}
	public override void OnDeactive(){
		menu.enabled = false;
	}
}
