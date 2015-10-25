using UnityEngine;
using System.Collections;
namespace Artwave.GameState{
	/**
	 *  odpowiada za obsługę maszyny stanów które przenikają się przez wszystkie sceny.
	 * Na potrzeby danej gry użytkownik buduje sobie StateController który dziedziczy po AbstractStateController. Przypisuje go do tego samego obiektu i ustawia powiązanie z managerem.
	 * Sam manager nie zarządza stanami. On zarządza StateControllerem który zarządza stanami. Manager zarządza też przełączaniem między scenami
	 * 
	 * Wszystkie komponenty które mają być nieśmiertelne muszą znaleźć się też w tym samym obiekcie
	 * W celu lepszego testowania w każdej scenie najlepiej utworzyć ten obiekt. Wtedy gdy przechodzimy międzyscenami to ta sama instancja się przenosi i usuwa duplikaty. Jeśli jednak odpalimy samę inną scenę to odpali się
	 * ten obiekt z danej sceny
	 */ 
	public class StateManager : MonoBehaviour {
//		public AbstractState activeState;
		/**
		 * tworzymy z GameObject by był niezniszczalny. Będzie przenoszony między scenami
		 */ 
		private static StateManager instance;
		public static StateManager GetManager(){
			return instance;
		}
		/**
		 * stateManager sprawdza czy level jest wczytany. Poniższa zmienna określa czy w momencie jak wykryje że level się wczytał ma poinformować kontroler
		 * Zastosowanie przy wczytywaniu nowej sceny. Kontroler inicjiuje i czeka aż manager poinformuje go że wszystko gotowe
		 */ 
		private bool notifyLevelLoad;
		void Awake(){
			notifyLevelLoad = false;
			if(instance==null){
				instance=this;
				DontDestroyOnLoad(this.gameObject);
				InitStateController();
			}else{
				DestroyImmediate(this.gameObject);
			}
		}
		void Start(){}
		/**
		 *  Do każdej gry musimy stworzyć StateController który dziedziczy po abstractStateController, dodajemy go najlepiej do tego samego obiektu co jest w nim StateManager
		 */ 
		public AbstractStateController stateController;
		public void InitStateController(){
			if (stateController == null) {
				stateController = GetComponent<AbstractStateController>();
			}

			if (stateController != null) {
				if(stateController.gameObject != this.gameObject){
					Debug.LogError("StateManager and StateController gameobject reference are not equal");
				}
				stateController.Init(this);
			} else {
				Debug.LogError("StateManager should have StateController reference");
			}
		}
		public static AbstractStateController GetController(){
			return instance.stateController;
		}
		/**
		 * pozwala wczytać nowy stan
		 * bezpośrednie wywołanie tej metody oznacza że nowy stan jest na tej samej scenie
		 */ 
		public static void ChangeState(string stateName){
			instance.stateController.ChangeState (stateName);
		}
		/**
		 * pozwala wczytać inną scenę i określony stan na tej scenie
		 */ 
		public static void ChangeState(string scene, string stateName){
			instance.stateController.ChangeState(scene, stateName);
		}
		/**
		 * pozwala przekazywać wiadomości-eventy do kontrolera i do aktywnego stanu.
		 * Aktywny stan z otoczeniem powinien się eventowo w ten sposób porozumiewać.
		 */ 
		public static void SendEvent(string message){
			instance.stateController.SendEvent(message);
		}
		void Update(){
			if (Application.isLoadingLevel == false) { // znaczy że level jest wczytany
				if(notifyLevelLoad){//jak level wczytany i ma być przekazana infromacja do SateControlelra to informujemy go i wyłączamy notify.
					stateController.ChangeSceneSuccess();
					notifyLevelLoad = false;
				}
			}
		}
		public void ChangeScene(string scene){
			notifyLevelLoad = true;
			Application.LoadLevel (scene);
		}
	}
}
