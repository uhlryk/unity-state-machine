using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Artwave.GameState{
	/**
	 *  klasa z której dziedziczyć ma StateController. StateController jest specyficzny do każdej gry. Ma on zarządzać stanami. Ma dostęp do managera. ma możliwość zmieniania stanów i zmieniania scen
	 * Jeśli jakiś kod ma być uruchomiony tylko raz na całą grę używamy public override void OnInit() jeśli raz na scenę to void Start () co update void Update () {
	 */ 
	public abstract class AbstractStateController : MonoBehaviour{
		public AbstractState activeState;
		private StateManager manager;
		public void Init (StateManager manager){
			this.manager = manager;
			if (activeState != null) {
				isInitStateInUpdate = true;
			}
			OnInit();
		}
		/**
		 * określa czy aktywny state ma zostać aktywowany w fazie void Update. Najpierw był w Init ale init jest w Awake, a to za wcześnie. Może być tak że controller nie ma activeState w swojej fazie init więc nie ma 
		 * w takiej sytuacji problemu
		 */ 
		private bool isInitStateInUpdate = false;
		void Update(){
			if (isInitStateInUpdate == true) {
				isInitStateInUpdate = false;
				activeState.Activate();
			}
		}
		/**
		 * linkuje nowy stan i go aktywuje
		 */ 
		private void NewState(string stateName){
			AbstractState state = null;
			string[] stateNameArray = stateName.Split ('.');
			if (stateNameArray.Length > 1) { // znaczy się że mamy strukturę obiekt.komponentStanu a scena obecna
				GameObject stateObject = GameObject.Find (stateNameArray [0]);
				if (stateObject) {
					state = stateObject.GetComponent (stateNameArray [1]) as AbstractState;
				} else {
					if(state == null){
						Debug.LogError("There is no such state Object");
					}
				}
			} else {
				Debug.LogError("There is no such state");
			}
			if(state == null){
				Debug.LogError("There is no such state");
			}
			activeState = state;
			activeState.Activate ();
		}

		public AbstractState GetState(){
			return activeState;
		}
		/**
		 * ustawiane zwykle w changescene. Określamy jaka ma być nowa scena która zacznie być odpalana gdy nowa scena się wczyta
		 */ 
		private string newStateNameInNewScene; 
		/**
		 * pozwala wczytać nowy stan
		 * bezpośrednie wywołanie tej metody oznacza że nowy stan jest na tej samej scenie
		 */ 
		public void ChangeState(string stateName){
			if (OnChangeStateStart ()) {
				if (activeState != null) {
					activeState.Deactivate ();
				}
				NewState (stateName);
			}
		}
		/**
		 * pozwala wczytać inną scenę i określony stan na tej scenie
		 */ 
		public void ChangeState(string scene, string stateName){
			if (OnChangeStateStart ()) {
				if (activeState != null) {
					activeState.Deactivate ();
				}
				newStateNameInNewScene = stateName;
				manager.ChangeScene (scene);
			}
		}
		/**
		 * manager poinformował że scena jest załadowana
		 * wtedy kontroler odpowiada za znalezienie stanu i musi go przekazać managerowi
		 */ 
		public void ChangeSceneSuccess(){
			this.OnChangeScene ();
			NewState (newStateNameInNewScene);
		}
		/**
		 * pozwala przekazywać wiadomości-eventy do kontrolera i do aktywnego stanu.
		 * Aktywny stan z otoczeniem powinien się eventowo w ten sposób porozumiewać.
		 */ 
		public void SendEvent(string message){
			if (OnReceiveEventStart (message)) {
				if (activeState != null) {
					activeState.SendEvent (message);
				}
				OnReceiveEventEnd ();
			}
		}
		/**
		 * odpala się tylko raz gdy manager inicjuje statecontroller. Jeśli controlelr miał ustawiony state, to najpierw state się aktywuje
		 */ 
		public virtual void OnInit(){}
		/**
		 * odpala gdy manager poinformuje że scena załadowana
		 */ 
		public virtual void OnChangeScene(){}
		/**
		 * odpala się jeszcze przed deaktywacją poprzedniego stanu. Jeśli false to nie dojdzie do zmiany stanu
		 */ 
		public virtual bool OnChangeStateStart(){
			return true;
		}
		/**
		 * odpala się na samym końcu zaraz po aktywacji nowej sceny (jest dostępna)
		 */ 
		public virtual void OnChangeStateEnd(){}
		/**
		 * odpala się gdy jakiś obiekt sceny wyśle do kontrolera informacje o zdarzeniu
		 * Odpala się zanim się jeszcze dany stan o tym dowie
		 * Jeśli false to stan się o niczym nie dowie
		 */ 
		public virtual bool OnReceiveEventStart(string message){
			return true;
		}
		/**
		 * odpala się gdy jakiś obiekt sceny wyśle do kontrolera informacje o zdarzeniu
		 * Odpala się zanim się po tym jak dany stan o tym dowie
		 */ 
		public virtual void OnReceiveEventEnd(){}
	}
}
