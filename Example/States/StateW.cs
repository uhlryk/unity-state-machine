using UnityEngine;
using System.Collections;
using Artwave.GameState;
public class StateW : AbstractState {
	private Menu1 menu;
	public bool jakisParametr;
	public string jakisInnyParametr;
	public override void OnActive(){
		Debug.Log ("Aktywacja StateW");
		menu = GameObject.Find ("GUI").GetComponent("Menu1") as Menu1;
		menu.enabled = true;

	}
	public override void OnReceiveEvent(string message){
		Debug.Log ("Event StateW " + message);

		if (message == "GotoXButtonClick") {
			StateManager.ChangeState ("StateX.StateX");
		}
	}
	public override void OnDeactive(){
		menu.enabled = false;
	}
}
