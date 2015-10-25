using UnityEngine;
using System.Collections;
using Artwave.GameState;
public class StateX : AbstractState {
	public override void OnActive(){
		Debug.Log ("Aktywacja StateX");
		StateManager.ChangeState ("SceneB","StateY.StateY");
	}
}
