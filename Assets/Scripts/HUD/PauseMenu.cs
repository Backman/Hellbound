using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu{
	private static PauseMenu m_Instance = null;
	private List<PauseMenuController> m_Controllers;
	
	private PauseMenu(){
		m_Controllers = new List<PauseMenuController>();
	}
	
	public static PauseMenu getInstance(){
		if(m_Instance == null){
			m_Instance = new PauseMenu();
		}
		return m_Instance;
	}
	
	public void hideAll(){
		foreach(PauseMenuController controller in m_Controllers){
			controller.hide();
		}
	}
	
	public void add(PauseMenuController controller){
		m_Controllers.Add(controller);
	}
}
