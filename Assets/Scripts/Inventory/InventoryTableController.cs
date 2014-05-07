using UnityEngine;
using System.Collections;

/// <summary>
/// Script that need to be added to the inventory UIGrid
/// because Unity does not update the scrollview....
/// By Arvd/Aleksi 2014-04-16
/// </summary>
public class InventoryTableController : MonoBehaviour {

	private static bool m_DoReposition = false;
	private static UITable r_Table;
	private static UIScrollView r_ScrollView;
	
	void Start(){
		r_Table = (UITable)GetComponent(typeof(UITable));
	}
	
	/// <summary>
	/// Needs to be called after a item has been added to the inventory.
	/// Unity is a mother fucking bullshit engine
	/// </summary>
	public static void reposition(){
		m_DoReposition = true;
	}

	void LateUpdate(){
		if( m_DoReposition ){
			r_Table.Reposition();
			m_DoReposition = false;
		}
	}


}
