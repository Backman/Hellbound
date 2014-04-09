using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PressurePlate : Interactable {
	public enum Symbol {
		Earth,
		Water,
		Fire,
		Wind
	}
	
	public Symbol m_Symbol;
	private StateMachine<PressurePlate> m_Machine;
	//public PressurePlate test;
	// Use this for initialization
	void Start () {
		Messenger.AddListener<bool>("notify earth symbol", notifyEarth);
		Messenger.AddListener<bool>("notify fire symbol", notifyFire);
		Messenger.AddListener<bool>("notify water symbol", notifyWater);
		Messenger.AddListener<bool>("notify wind symbol", notifyWind);

		m_Machine = new StateMachine<PressurePlate>(this, new GoodState());
		renderer.material.color = Color.green;
		m_Machine.addState (new BadState());
	}
	
	// Update is called once per frame
	void Update () {
		m_Machine.update(Time.deltaTime);
	}

	void OnTriggerEnter(Collider col) {
		if(m_ActivateType == ActivateType.OnTrigger){
			if(col.tag == "Player") {
				activate();
			}
		}
	}

	/*public void reason() {
		m_Machine.CurrentState.reason(this);
	}*/

	public override void activate () {
		m_Machine.CurrentState.activate(this);
	}

	public S changeState<S>() where S : State<PressurePlate> {
		return m_Machine.changeState<S>() as S;
	}

	public void notifyEarth(bool bad) {
		if(m_Symbol != Symbol.Earth) {
			return;
		}
		if(bad) {
			m_Machine.changeState<BadState>();
			return;
		}
		
		m_Machine.changeState<GoodState>();
	}

	public void notifyFire(bool bad) {
		if(m_Symbol != Symbol.Fire) {
			return;
		}
		if(bad) {
			m_Machine.changeState<BadState>();
			return;
		}
		
		m_Machine.changeState<GoodState>();
	}

	public void notifyWater(bool bad) {
		if(m_Symbol != Symbol.Water) {
			return;
		}
		if(bad) {
			m_Machine.changeState<BadState>();
			return;
		}
		
		m_Machine.changeState<GoodState>();
	}

	public void notifyWind(bool bad) {
		if(m_Symbol != Symbol.Wind) {
			return;
		}
		if(bad) {
			m_Machine.changeState<BadState>();
			return;
		}
		
		m_Machine.changeState<GoodState>();
	}
}
