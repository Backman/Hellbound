using UnityEngine;
using System.Collections;

public class MenuController {
	public enum State
	{
		START,
		PAUSE
	}
	private static MenuController m_Instance = null;
	private State m_State;
	private MenuController(){
		m_State = State.START;
	}
	
	public static MenuController getInstance(){
		if(m_Instance == null){
			m_Instance = new MenuController();
		}
		return m_Instance;
	}
	
	public MenuController.State getState(){
		return m_State;
	}
}
