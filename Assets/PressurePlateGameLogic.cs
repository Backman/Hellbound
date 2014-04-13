using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SymbolBool {
	public bool Earth;
	public bool Water;
	public bool Wind;
	public bool Fire;
}

public class PressurePlateGameLogic : MonoBehaviour {

	public SymbolBool Earth = new SymbolBool();
	public SymbolBool Water = new SymbolBool();
	public SymbolBool Wind = new SymbolBool();
	public SymbolBool Fire = new SymbolBool();

	public void Start() {
		Messenger.AddListener<PressurePlate.Symbol>("plate triggered", plateTriggered);
	}

	public void plateTriggered(PressurePlate.Symbol symbol) {
		switch(symbol) {
		case PressurePlate.Symbol.Earth:
			handleEarthSymbol();
			break;
			
		case PressurePlate.Symbol.Fire:
			handleFireSymbol();
			break;
			
		case PressurePlate.Symbol.Wind:
			handleWindSymbol();
			break;
			
		case PressurePlate.Symbol.Water:
			handleWaterSymbol();
			break;
		}
	}

	private void handleEarthSymbol() {
		Messenger.Broadcast<bool>("notify earth symbol", Earth.Earth);
		Messenger.Broadcast<bool>("notify fire symbol", Earth.Fire);
		Messenger.Broadcast<bool>("notify water symbol", Earth.Water);
		Messenger.Broadcast<bool>("notify wind symbol", Earth.Wind);
	}

	private void handleFireSymbol() {
		Messenger.Broadcast<bool>("notify earth symbol", Fire.Earth);
		Messenger.Broadcast<bool>("notify fire symbol", Fire.Fire);
		Messenger.Broadcast<bool>("notify water symbol", Fire.Water);
		Messenger.Broadcast<bool>("notify wind symbol", Fire.Wind);
	}

	private void handleWaterSymbol() {
		Messenger.Broadcast<bool>("notify earth symbol", Water.Earth);
		Messenger.Broadcast<bool>("notify fire symbol", Water.Fire);
		Messenger.Broadcast<bool>("notify water symbol", Water.Water);
		Messenger.Broadcast<bool>("notify wind symbol", Water.Wind);
	}

	private void handleWindSymbol() {
		Messenger.Broadcast<bool>("notify earth symbol", Wind.Earth);
		Messenger.Broadcast<bool>("notify fire symbol", Wind.Fire);
		Messenger.Broadcast<bool>("notify water symbol", Wind.Water);
		Messenger.Broadcast<bool>("notify wind symbol", Wind.Wind);
	}
}
