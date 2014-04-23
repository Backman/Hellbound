using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Singleton class used for switching between Inventory and Settings
/// in pause menu.
/// </summary>
public class PauseMenu{
	private static PauseMenu m_Instance = null;
	private List<PauseMenuController> r_Controllers;
	
	private PauseMenu(){
		r_Controllers = new List<PauseMenuController>();
	}
	
	public static PauseMenu getInstance(){
		if(m_Instance == null){
			m_Instance = new PauseMenu();
		}
		return m_Instance;
	}
	
	public void hideAll(){
		foreach(PauseMenuController controller in r_Controllers){
			controller.hide();
		}
	}
	
	public void add(PauseMenuController controller){
		r_Controllers.Add(controller);
	}

	public void showJournal(){
		foreach(PauseMenuController controller in r_Controllers){
			if(controller.name == "Journal"){
				controller.show();
			}
			else{
				controller.hide();
			}
		}
	}

	public void showPauseWindow(){
		foreach(PauseMenuController controller in r_Controllers){
			if(controller.name == "Pause"){
				InventoryTableController.reposition ();
				controller.show();
				InventoryTableController.reposition ();
			}
			else{
				controller.hide();
			}
		}
	}
}
