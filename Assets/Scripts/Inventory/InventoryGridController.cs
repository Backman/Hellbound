﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Script that need to be added to the inventory UIGrid
/// because Unity does not update the scrollview....
/// By Arvd/Aleksi 2014-04-16
/// </summary>
public class InventoryGridController : MonoBehaviour {
	
	private static UIGrid r_Grid;
	private static UIScrollView r_ScrollView;
	
	void Start(){
		r_Grid = (UIGrid)GetComponent(typeof(UIGrid));
		r_ScrollView = (UIScrollView)r_Grid.transform.parent.GetComponent(typeof(UIScrollView));
	}
	
	/// <summary>
	/// Needs to be called after a item has been added to the inventory.
	/// Unity is a mother fucking bullshit engine
	/// </summary>
	public static void reposition(){
		r_Grid.Reposition ();
		r_ScrollView.UpdatePosition();
	}
}