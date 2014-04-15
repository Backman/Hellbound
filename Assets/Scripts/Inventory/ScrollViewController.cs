using UnityEngine;
using System.Collections;

public class ScrollViewController : MonoBehaviour {

	private static UIScrollView r_ScrollView;

	// Use this for initialization
	void Awake () {
		r_ScrollView = GetComponent<UIScrollView>();
	}

	public static void updateScrollView(){
		r_ScrollView.OnScrollBar ();
	}
}
