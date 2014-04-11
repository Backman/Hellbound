using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class InteractableDataBase : Singleton<InteractableDataBase> {

	InteractableDataBase[] m_List;
	bool m_IsDirty = true;

	public List<InteractableBlueprint> m_InteractableList = new List<InteractableBlueprint>();

	public InteractableDataBase[] List { get { return m_List; } }

	public UIAtlas m_IconAtlas;

	private InteractableBlueprint getInteractable(int id) {
		for(int i = 0; i < m_InteractableList.Count; ++i) {
			InteractableBlueprint ib = m_InteractableList[i];
			if(ib.m_ID == id) {
				return ib;
			}
		}
		return null;
	}

	private InteractableBlueprint getInteractable(string name) {
		for(int i = 0; i < m_InteractableList.Count; ++i) {
			InteractableBlueprint ib = m_InteractableList[i];
			if(ib.m_Name == name) {
				return ib;
			}
		}

		return null;
	}

	public InteractableBlueprint findByID(int id) {
		return getInteractable(id);
	}

	public InteractableBlueprint findByName(string name) {
		return getInteractable(name);
	}
}
