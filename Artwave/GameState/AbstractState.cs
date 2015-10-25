using UnityEngine;
using System.Collections;


namespace Artwave.GameState{
	/**
	 *  każdy stan ma swój game obiekt który jest wewnątrz obiektu GameManager. Domyślnie są wyłączone.
	 * aktywny jest tylko jeden stan. Który jest przypięty do stateManagera. Każdy stan musi mieć możliwość oprócz inicjacji wyczyszczenia swoich danych.
	 * 
	 * Uwaga nie należy polegać na void Start
	 * Działa poprawnie ale Start działa gdy komponent jest aktywny na scenie. A stan może być nieaktywny ale jako komponent na scenie jest aktywny. 
	 * Z wiązku z czym odpalą się wszystkie stany przez Start.
	 * Jak chcemy reagować na aktywację stanu to używać należy OnActive
	 */ 
	public abstract class AbstractState : MonoBehaviour {
		private bool isActive = false;
		public void Activate(){
			isActive = true;
			OnActive ();
		}
		public void Deactivate(){
			isActive = false;
			OnDeactive ();
		}
		public bool IsActive(){
			return isActive;
		}
		public void SendEvent(string message){
			OnReceiveEvent (message);
		}
		public virtual void OnActive(){}
		public virtual void OnDeactive(){}
		public virtual void OnReceiveEvent(string message){}
	}
}
