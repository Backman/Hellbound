using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {
	/// <summary>
	/// This script needs to be placed on the character (more specifically the object that
	/// have the player collider)
	/// </summary>


	public float f_AlterationSpeed = 0.25f;

	private FMOD.Studio.ParameterInstance f_Parameter;

	private List<int> i_AllInts = new List<int>();
	private List<int> i_UniqueInts = new List<int>();
	private int i_CurrentInt = 0;

	private float f_DesiredParameter = 0f;
	private float f_ActualParameter = 0f;

	private bool b_AlterSound = false;

	void Start () 
	{
		f_Parameter = gameObject.GetComponent<FMOD_StudioEventEmitter> ().getParameter("");
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<MusicZone>() != null)
		{
			i_AllInts.Add(other.GetComponent<MusicZone>().m_Zone);

			CheckValues();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<MusicZone>() != null)
		{
			i_AllInts.Remove(other.GetComponent<MusicZone>().m_Zone);

			CheckValues();
		}
	}

	private void CheckValues()
	{
		i_UniqueInts.Clear ();
		foreach(int i in i_AllInts)
		{
			if(!i_UniqueInts.Contains(i))
				i_UniqueInts.Add(i);
		}

		if(i_UniqueInts.Count == 1)
		{
			if(i_UniqueInts[0] == 0)
			{
				f_DesiredParameter = 0f;
				b_AlterSound = true;
			}
			else if(i_UniqueInts[0] == 1)
			{
				f_DesiredParameter = 100f;
				b_AlterSound = true;
			}
		}
		else if(i_UniqueInts.Count > 1)
		{
			f_DesiredParameter = 50f;
			b_AlterSound = true;
		}
	}

	void Update()
	{
		if(b_AlterSound)
		{
			float moveTo = f_DesiredParameter - f_ActualParameter;
			if(moveTo > 0)
			{
				f_ActualParameter += f_AlterationSpeed;
			}
			else if(moveTo < 0)
			{
				f_ActualParameter -= f_AlterationSpeed;
			}
			else 
			{
				b_AlterSound = false;
			}
		}
	}
}
