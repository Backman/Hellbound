using UnityEngine;
using System.Collections;

public class Condition : MonoBehaviour {

	[SerializeField] private bool m_IsMet = false;

	public bool isMet 
	{
		get { return m_IsMet; }
		set { m_IsMet = value; }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
