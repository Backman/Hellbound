#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PolyCollider : MonoBehaviour
{

    #region Enum

    public enum ColliderType
    {
		Standard,
		SafeZone,
        DamageZone
    }

    #endregion
    #region Members

    private bool _isEditable = true;
	
    public ColliderType _colliderType = ColliderType.Standard;

    public float _yPosition = 0.0f;
	public float _timer = 0.0f;

    public float _awakeTime;
    public float _startTime;
    public float _endTime;
    public float _deathTime;

    public Vector2 _startPosition;
    public Vector2 _endPosition;

    public List<KeyValuePair<Vector2, Vector2>> _startEndPoints = new List<KeyValuePair<Vector2, Vector2>>();
    //public List<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>> _points = new List<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>>();

	public List<Vector2> _points = new List<Vector2>();

    public bool IsEditable
    {
        get
        {
            return _isEditable;
        }
    }

    public bool IsValid
    {
        get
        {
			return _points.Count > 2;
        }
    }

#if UNITY_EDITOR

    public int lastIndex = -1;
    public bool isDraggingPoint = false;
    public Vector2 handleOffset = Vector2.zero;

#endif

    #endregion

    #region Constructor

    public static PolyCollider CreateInstance()
    {
        GameObject go = new GameObject();
        go.name = "PolyCollider" + go.GetInstanceID();

        return go.AddComponent<PolyCollider>();
    }

    #endregion

	public bool IsConvex() 
	{
		if(_points.Count < 3)
		{
			return false;
		}

		Vector2 p;
		Vector2 v;
		Vector2 u;
		float res = 0.0f;
		for(int i = 0; i < _points.Count; ++i) 
		{
			p = _points[i];
			Vector2 temp = _points[(i+1) % _points.Count];

			v = temp - p;

			u = _points[(i+2) % _points.Count];

			if(i == 0) // In the first loop the direction is unkown..
			{
				res = (u.x * v.y) - (u.y * v.x) + (v.x * p.y)  - (v.y * p.x);
			}
			else
			{
				float newRes = (u.x * v.y) - (u.y * v.x) + (v.x * p.y) - (v.y * p.x);
				if((newRes > 0 && res < 0) || (newRes < 0 && res > 0))
				{
					return false;
				}
			}
		}
		return true;
	}

    #region Editing

    public void SetEditable(bool editable)
    {
        _isEditable = editable;
    }

    public int AddPoint(Vector2 point, int insertPoint)
    {
		if (insertPoint < 0 || insertPoint > _points.Count - 1)
        {
            Vector3 p = transform.InverseTransformPoint(point.ToVector3(_yPosition));
            Vector2 pos = new Vector2(p.x, p.z);

            KeyValuePair<Vector2, Vector2> values = new KeyValuePair<Vector2,Vector2>(pos, pos);
			_startEndPoints.Add(values);

			_points.Add(pos);
        }
        else
        {
            Vector3 p = transform.InverseTransformPoint(point.ToVector3(_yPosition));
            Vector2 pos = new Vector2(p.x, p.z);

            KeyValuePair<Vector2, Vector2> values = new KeyValuePair<Vector2, Vector2>(pos, pos);
			_startEndPoints.Insert(insertPoint, values);

			_points.Insert(insertPoint, pos);
        }

		return (insertPoint < 0 || insertPoint > _points.Count - 1) ? _points.Count - 1 : insertPoint;
    }

    public void SetPoint(int index, Vector2 point)
    {
		if (index > -1 && index < _points.Count)
        {
			Vector3 p = transform.InverseTransformPoint(point.ToVector3(_yPosition));
            Vector2 pos = new Vector2(p.x, p.z);
            
			_points[index] = pos;
        }
    }

    public void SetStartPosition(int index, Vector2 pos)
    {
		KeyValuePair<Vector2, Vector2> values = new KeyValuePair<Vector2, Vector2>(pos, _startEndPoints[index].Value);
		_startEndPoints[index] = values;
    }

    public void SetEndPosition(int index, Vector2 pos)
    {
		KeyValuePair<Vector2, Vector2> values = new KeyValuePair<Vector2, Vector2>(_startEndPoints[index].Key, pos);
		_startEndPoints[index] = values;
    }

    public Vector2 GetStartPosition(int index)
    {
			return _startEndPoints[index].Key;
    }

    public Vector2 GetEndPosition(int index)
    {
		return _startEndPoints[index].Value;
    }

    public Vector2 GetPosition(int index)
    {
		return _points[index];
    }

    public void RemovePointAtIndex(int index)
    {
		if (index > -1 && index < _points.Count)
        {
			_points.RemoveAt(index);
        }
		if (index > -1 && index < _startEndPoints.Count)
        {
			_startEndPoints.RemoveAt(index);
        }
    }

    public void ClearPoints()
    {
		_points.Clear();
		_startEndPoints.Clear();
    }

    public void Refresh()
    {
        if (_points.Count < 1)
        {
            return;
        }

		/*if(!IsConvex ())
		{
			Debug.Log ("The polygon is not convex..");
		}*/
    }

    #endregion

    protected void Awake()
    {
        Refresh();
    }

    protected void Start()
    {
        Refresh();
    }

	void OnEnable()
	{
#if UNITY_EDITOR
		SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
	}

	void OnDisable()
	{
#if UNITY_EDITOR
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
	}

#if UNITY_EDITOR
	void OnSceneGUI(SceneView sceneView)
	{
		Color outLineColor = GetTypeColor();
		float r, g, b;
		r = outLineColor.r;
		g = outLineColor.g;
		b = outLineColor.b;
		Color fillColor = new Color(r, g, b, 0.1f);
		Vector3[] v = new Vector3[_points.Count];
		int pCount = v.Length / 4;
		for(int i = 0; i < v.Length; i++)
		{
			v[i] = _points[i].ToVector3(_yPosition);
		}
		if(v.Length > 2)
		{
			Handles.color = GetTypeColor();
			Handles.DrawPolyLine (v);
			Handles.DrawLine(v[v.Length - 1], v[0]);
			Handles.color = Color.white;
			SceneView.RepaintAll();
		}
	}
#endif

    private void SetGizmosColor()
    {
        switch (_colliderType)
        {
		case ColliderType.SafeZone:
			Gizmos.color = Color.green;
			break;
		case ColliderType.Standard:
			Gizmos.color = Color.blue;
			break;

		case ColliderType.DamageZone:
			Gizmos.color = Color.red;
			break;
        }
    }

	public Color GetTypeColor()
	{
		switch (_colliderType)
		{
		case ColliderType.SafeZone:
			return Color.green;

		case ColliderType.Standard:
			return Color.blue;
			
		case ColliderType.DamageZone:
			return Color.red;
		}

		return Color.white;
	}

	[ContextMenu("Generate Mesh Collider")]
	public void generateMeshCollider() {
		List<Vector2> vertices2D = new List<Vector2> ();
		PolyCollider polyCollider = GetComponent<PolyCollider> ();
		
		if (polyCollider == null)
			return;
		
		foreach (Vector2 point in polyCollider._points) {
			vertices2D.Add(point);
		}
		
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices2D.ToArray());
		int[] indices = tr.Triangulate();
		
		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertices2D.Count];
		for (int i=0; i<vertices.Length; i++) {
			vertices[i] = new Vector3(vertices2D[i].x, 0.0f, vertices2D[i].y);
			
			Ray ray = new Ray(vertices[i], Vector3.up );
			RaycastHit[] hit = Physics.RaycastAll(ray, 10.0f);
			foreach( RaycastHit h in hit ){
				if( h.transform.gameObject.tag == "RaycastTerrain" )
					vertices[i].y = h.point.y + 0.1f;
			}
		}
		
		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();
		
		// Set up game object with mesh;
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider> ();
		
		meshCollider.sharedMesh = msh;
	}
}
