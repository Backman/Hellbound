using UnityEngine;
using System.Collections;

public class TipsHandler : MonoBehaviour {

	
	[System.Serializable]
	public class Tips{
		[SerializeField][Multiline]
		public string Title;
		
		[SerializeField][Multiline]
		public string Text;
	}
	
	public Tips[] tips;
}
