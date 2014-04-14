using UnityEngine;
using System.Collections;

public class PuzzleFallerPlatform : PuzzleFaller {

	[SerializeField] int m_CurrentUsage = 1;
	[SerializeField] int m_MaxUsage = 1;
	//To set parent for the puzzle, change the parent of the gameObject that
	//this script is attached to
	private PuzzleFaller m_Parent;
	//<summary>
	//maxUsage has to be 1 or higher
	//</summary>
	void Start(){
		m_Parent = gameObject.transform.parent.gameObject.GetComponent<PuzzleFaller>();
	}
	
	public bool usable(){
		return m_CurrentUsage < m_MaxUsage;
	}
	
	public void use(){
		++m_CurrentUsage;
		m_Parent.checkConditions();
	}
}
