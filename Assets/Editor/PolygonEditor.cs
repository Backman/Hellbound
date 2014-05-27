using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PolyCollider))]
public class PolygonEditor : Editor
{

    #region Members

    const int SCENEVIEW_HEADER = 40;
    const int HANDLE_SIZE = 32;
    const int INSERT_HANDLE_SIZE = 24;
    private Texture2D handleIconNormal;
    private Texture2D handleIconActive;
    private Texture2D deleteIconNormal;
    private Texture2D deleteIconActive;
    private Texture2D insertIconNormal;
    private Texture2D insertIconActive;
    private GUIStyle insertIconStyle;
    private GUIStyle deletePointStyle;

    private PolyCollider poly;
	private float polyStartEndDiff;

    public int insertPoint = -1;
    public int activePoint = 0;
    public bool pointSelected = false;

    public bool snapEnabled;
    public float snapValue;
    public PolyCollider.ColliderType colliderType;
    public float timeSlider;

    public float awakeTime;
    public float startTime;
    public float endTime;
    public float deathTime;

    public Vector2 startPosition;
    public Vector2 endPosition;

    #endregion


    #region Convenient

    public bool EarlyOut
    {
        get
        {
            return (Event.current.alt ||
                    Tools.current == Tool.View ||
                    GUIUtility.hotControl > 0 ||
                    (Event.current.isMouse ? Event.current.button > 1 : false) ||
                    Tools.viewTool == ViewTool.FPS ||
                    Tools.viewTool == ViewTool.Orbit);
        }
    }

    #endregion

    [MenuItem("TarHead/Level Editor/PolyCollider/PolyCollider Object %#t")]
    public static void GameObjectInit()
    {
        CreatePolyColliderObject();
    }

    public static void CreatePolyColliderObject()
    {
        PolyCollider polyCollider = PolyCollider.CreateInstance();
		GameObject polyParent = GameObject.Find("PolyColliders");
		if(!polyParent)
		{
			polyParent = new GameObject("PolyColliders");
		}
		polyCollider.transform.parent = polyParent.transform;
        Selection.activeTransform = polyCollider.transform;
    }

    private void OnEnable()
    {
        handleIconNormal = (Texture2D)Resources.LoadAssetAtPath("Assets/Resources/Icons/HandleIcon-Normal.png", typeof(Texture2D));
        handleIconActive = (Texture2D)Resources.LoadAssetAtPath("Assets/Resources/Icons/HandleIcon-Active.png", typeof(Texture2D));

        deleteIconNormal = (Texture2D)Resources.LoadAssetAtPath("Assets/Resources/Icons/DeletePoint-Normal.png", typeof(Texture2D));
        deleteIconActive = (Texture2D)Resources.LoadAssetAtPath("Assets/Resources/Icons/DeletePoint-Active.png", typeof(Texture2D));

        insertIconNormal = (Texture2D)Resources.LoadAssetAtPath("Assets/Resources/Icons/InsertPoint-Normal.png", typeof(Texture2D));
        insertIconActive = (Texture2D)Resources.LoadAssetAtPath("Assets/Resources/Icons/InsertPoint-Active.png", typeof(Texture2D));

        insertIconStyle = new GUIStyle();
        insertIconStyle.normal.background = insertIconNormal;
        insertIconStyle.active.background = insertIconActive;

        deletePointStyle = new GUIStyle();
        deletePointStyle.normal.background = deleteIconNormal;
        deletePointStyle.active.background = deleteIconActive;

        if (Undo.undoRedoPerformed != UndoRedoPerformed)
        {
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

		timeSlider = 0.0f;

        poly = (PolyCollider)target;
        snapEnabled = EditorPrefs.HasKey("polyCollider_snapEnabled") ? EditorPrefs.GetBool("polyCollider_snapEnabled") : false;
        snapValue = EditorPrefs.HasKey("polyCollider_snapValue") ? EditorPrefs.GetFloat("polyCollider_snapValue") : 0.25f;
    
	
		awakeTime = poly._awakeTime;
		deathTime = poly._deathTime;
		startTime = poly._startTime;
		endTime = poly._endTime;
		
		polyStartEndDiff = startTime - endTime;
	}

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= UndoRedoPerformed;
    }

    public override void OnInspectorGUI()
    {
        string onOff = poly.IsEditable ? "Lock Editing" : "Edit PolyCollider Object";
        GUI.backgroundColor = poly.IsEditable ? Color.red : Color.green;
        if(GUILayout.Button(onOff, GUILayout.MinHeight(25)))
        {
            ToggleEditingEnabled();
        }
        GUI.backgroundColor = Color.white;
		
		bool guiChanged = false;

        GUILayout.Space(5);

		guiChanged = GUIEditSettings();

        GUILayout.Space(5);

		guiChanged = GUIPositionSettings();

        GUILayout.Space(5);

        activePoint = poly.lastIndex;
        if (pointSelected)
        {
			guiChanged = GUIPointSettings();
        }

        if (guiChanged)
        {
            EditorUtility.SetDirty(poly);
            poly.Refresh();
            SceneView.RepaintAll();
        }

        GUILayout.Space(10);

        GUI.backgroundColor = Color.magenta;
        if (GUILayout.Button("Clear Points"))
        {
            poly.ClearPoints();
            SceneView.RepaintAll();
        }
        GUI.backgroundColor = Color.white;

    }

    private bool GUIEditSettings()
    {
        bool snap = snapEnabled;
        float val = snapValue;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Collider Type");
			poly._colliderType = (PolyCollider.ColliderType)EditorGUILayout.EnumPopup(poly._colliderType);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Snap Enabled");
            snap = EditorGUILayout.Toggle(snapEnabled);
        }
        GUILayout.EndHorizontal();

        val = EditorGUILayout.FloatField("Snap Value", snapValue);

		//time = EditorGUILayout.Slider("Current Time", timeSlider, startTime, endTime);


		bool ret = false;
        if (snapEnabled != snap)
        {
            SetSnapEnabled(snap);
			ret = true;
        }
        if (snapValue != val)
        {
            SetSnapValue(val);
        	ret = true;
		}

		return ret;
    }

    private bool GUIPositionSettings()
    {
		bool ret = false;
		float start = startTime;
		float end = endTime;
		float time = timeSlider;

		polyStartEndDiff = end - start;

		poly._awakeTime = EditorGUILayout.FloatField("Awake Time", Mathf.Clamp(poly._awakeTime, 0.0f, float.MaxValue));
		poly._deathTime = EditorGUILayout.FloatField("Death Time", Mathf.Clamp(poly._deathTime, 0.0f, float.MaxValue));

		start = EditorGUILayout.FloatField("Start Time", Mathf.Clamp(startTime, 0.0f, float.MaxValue));
		end = EditorGUILayout.FloatField("End Time", Mathf.Clamp(endTime, 0.0f, float.MaxValue));

		if(polyStartEndDiff > 0.0f) {
			float currentTime = startTime + (polyStartEndDiff * timeSlider);
			time = EditorGUILayout.Slider("Current Time ("+currentTime+")", timeSlider, 0.0f, 1.0f);
		}

		if(startTime != start) {
			setStartTime(start);
			ret = true;
		}
		if(endTime != end) {
			setEndTime(end);
			ret = true;
		}
		if(timeSlider != time)
		{
			setPositionBasedOnTimer(time);
		}
		return ret;
    }

    private bool GUIPointSettings()
    {

        //EditorGUILayout.Vector2Field("Start Position", poly.GetStartPosition(activePoint));
        //EditorGUILayout.Vector2Field("End Position", poly.GetEndPosition(activePoint));
		bool ret = false;
        GUILayout.Space(20.0f);
        GUI.backgroundColor = Color.cyan;
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(new GUIContent("Set Start Position", "Hotkey(W)"), GUILayout.MinHeight(25.0f)))
            {
                poly.SetStartPosition(activePoint, poly.GetPosition(activePoint));
				ret = true;
			}
            if (GUILayout.Button("Go To Start Position", GUILayout.MinHeight(25.0f)))
            {
                poly.SetPoint(activePoint, poly.GetStartPosition(activePoint));
            }
        }
        EditorGUILayout.EndHorizontal();
        
        
        
        GUILayout.Space(20.0f);
        GUI.backgroundColor = Color.yellow;

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(new GUIContent("Set End Position", "Hotkey(E)"), GUILayout.MinHeight(25.0f)))
            {
                poly.SetEndPosition(activePoint, poly.GetPosition(activePoint));
				ret = true;
			}
            if (GUILayout.Button("Go To End Position", GUILayout.MinHeight(25.0f)))
            {
                poly.SetPoint(activePoint, poly.GetEndPosition(activePoint));
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20.0f);

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Place All At Start Position"))
            {
                for (int i = 0; i < poly._points.Count; ++i)
                {
                    poly.SetPoint(i, poly.GetStartPosition(i));
                }
            }
            if (GUILayout.Button("Place All At End Position"))
            {
                for (int i = 0; i < poly._points.Count; ++i)
                {
                    poly.SetPoint(i, poly.GetEndPosition(i));
                }
            }
        }
        EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		{
			if (GUILayout.Button(new GUIContent("Set Values To Start Positions", "Hotkey(Shift + W)")))
			{
				setStartPositions();
				ret = true;
			}
			if (GUILayout.Button(new GUIContent("Set Values To End Positions", "Hotkey(Shift + E)")))
			{
				setEndPositions();
				ret = true;
			}
		}
		EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = Color.white;

		return ret;
    }

	void setEndPositions()
	{
		for (int i = 0; i < poly._points.Count; ++i)
		{
			poly.SetEndPosition(i, poly.GetPosition(i));
		}
	}

	void setStartPositions()
	{
		for (int i = 0; i < poly._points.Count; ++i)
		{
			poly.SetStartPosition(i, poly.GetPosition(i));
		}
	}

    public void OnSceneGUI()
    {
		Event e = Event.current;
		SceneView sceneView = SceneView.lastActiveSceneView;

        if (poly != null)
        {
//			if(poly._points.Count > 2)
//			{
//				Vector3[] v = new Vector3[poly._points.Count+1];
//				Handles.color = poly.GetTypeColor();
//
//				for(int i = 0; i < v.Length; ++i)
//				{
//					if(i < poly._points.Count)
//					{
//						v[i] = new Vector3(poly._points[i].x, poly._yPosition, poly._points[i].y);
//					}
//					else
//					{
//						v[i] = new Vector3(poly._points[0].x, poly._yPosition, poly._points[0].y);
//					}
//				}
//				Handles.DrawPolyLine(v);
//			}

			if(!poly.IsEditable)
			{
				sceneView.orthographic = false;
				return;
			}
		}

        if (!sceneView)
        {
            return;
        }

        sceneView.rotation = Quaternion.Euler(Vector3.right * 90.0f);
        sceneView.orthographic = true;

		Vector3[] points = poly.transform.ToWorldSpace(poly._points.ToVector3(poly._yPosition));

        ShortcutListener(e);

        // Draw handles at points
        DrawHandles(points);

        if (DrawInsertPointGUI(points))
        {
            return;
        }

        if (EarlyOut)
        {
            return;
        }

        // Gives us the control of the scene view
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(controlID);

        Tools.current = Tool.None;

        PointDrawStyleInput(sceneView.camera, e, points);

        //DrawLines(points);
    }

    public bool DrawInsertPointGUI(Vector3[] points)
    {
        Handles.BeginGUI();
        int n = 0;
        for (int i = 0; i < points.Length; ++i)
        {
            n = (i >= points.Length - 1) ? 0 : i + 1;
            Vector3 avg = (points[i] + points[n]) / 2.0f;
            Vector2 g = HandleUtility.WorldToGUIPoint(avg);


            Rect handleRect = new Rect(g.x - INSERT_HANDLE_SIZE / 2.0f, g.y - INSERT_HANDLE_SIZE / 2.0f, INSERT_HANDLE_SIZE, INSERT_HANDLE_SIZE);

            if (GUI.Button(handleRect, "", insertIconStyle))
            {
                Undo.RecordObject(poly, "Add Point");
                Vector2 pos = new Vector2(avg.x, avg.z);
                if (snapEnabled)
                {
                    poly.lastIndex = poly.AddPoint(Round(pos, snapValue), n);
                }
                else
                {
                    poly.lastIndex = poly.AddPoint(pos, n);
                }

                Handles.EndGUI();
                return true;
            }
        }

        Handles.EndGUI();

        return false;
    }

    private void ShortcutListener(Event e)
    {
        if (!e.isKey || e.type != EventType.KeyUp)
        {
            return;
        }

        if(e.keyCode == KeyCode.Return)
        {
            poly.SetEditable(false);
        }



        if (e.keyCode == KeyCode.Backspace)
        {
            if (poly.lastIndex < 0)
            {
                return;
            }

            Undo.RecordObject(poly, "Delete Point");
            poly.RemovePointAtIndex(poly.lastIndex);
            poly.Refresh();
        }

        if (e.keyCode == KeyCode.E)
        {
			if(e.shift)
				setEndPositions();
            else
				poly.SetEndPosition(activePoint, poly.GetPosition(activePoint));
        }
        if (e.keyCode == KeyCode.W)
		{
			if(e.shift)
				setStartPositions();
			else
            	poly.SetStartPosition(activePoint, poly.GetPosition(activePoint));
        }
    }

    private void DrawHandles(Vector3[] p)
    {
        Handles.BeginGUI();
        GUI.backgroundColor = Color.red;

        pointSelected = false;
        for(int i = 0; i < p.Length; ++i)
        {
            Vector2 g = HandleUtility.WorldToGUIPoint(p[i]);
            Rect handleRect = new Rect(g.x - HANDLE_SIZE / 2.0f, g.y - HANDLE_SIZE / 2.0f, HANDLE_SIZE, HANDLE_SIZE);
            if (i == poly.lastIndex)
            {
                float ro = Time.realtimeSinceStartup;
                ro = (ro % 360) * 100f;
                GUIUtility.RotateAroundPivot(ro, g);
                    GUI.Label(handleRect, handleIconActive);
                GUIUtility.RotateAroundPivot(-ro, g);

                pointSelected = true;
                if (GUI.Button(new Rect(g.x + 10.0f, g.y - 40.0f, 25.0f, 25.0f), "", deletePointStyle))
                {
                    Undo.RecordObject(poly, "Delete Point");

                    poly.RemovePointAtIndex(i);
                    poly.Refresh();
                }
            }
            else
            {
                GUI.Label(handleRect, handleIconNormal);
            }
        }

        GUI.backgroundColor = Color.white;
        Handles.EndGUI();

        SceneView.RepaintAll();
    }

    private void DrawLines(Vector3[] p)
    {
        if (p.Length < 2)
        {
            return;
        }

        for (int i = 0; i < p.Length - 1; ++i)
        {
            Handles.DrawLine(p[i], p[i + 1]);
        }
        Handles.DrawLine(p[p.Length - 1], p[0]);
    }

    private float Round(float val, float snap)
    {
        return snap * Mathf.Round(val / snap);
    }

    private Vector2 Round(Vector2 val, float snap)
    {
        return new Vector2(snap * Mathf.Round(val.x / snap), snap * Mathf.Round(val.y / snap));
    }

    private Vector3 Round(Vector3 val, float snap)
    {
        return new Vector3(snap * Mathf.Round(val.x / snap), snap * Mathf.Round(val.y / snap), snap * Mathf.Round(val.z / snap));
    }

    #region Draw Style Input

    private void PointDrawStyleInput(Camera cam, Event e, Vector3[] p)
    {
        if (!e.isMouse)
        {
            return;
        }

        switch (e.type)
        {
            case EventType.MouseDown:
                for (int i = 0; i < p.Length; ++i)
                {
                    Vector2 g = HandleUtility.WorldToGUIPoint(p[i]);
                    Rect handleRect = new Rect(g.x - HANDLE_SIZE / 2f, g.y - HANDLE_SIZE / 2f, HANDLE_SIZE, HANDLE_SIZE);

                    if (handleRect.Contains(e.mousePosition))
                    {
                        poly.isDraggingPoint = true;
                        poly.lastIndex = i;
                        poly.handleOffset = g - e.mousePosition;
                    }
                }

                if (!poly.isDraggingPoint)
                {
                    Undo.RecordObject(poly, "Add Point");

                    if (snapEnabled)
                    {
                        poly.lastIndex = poly.AddPoint(Round(GetWorldPoint(cam, e.mousePosition), snapValue), insertPoint);
                    }
                    else
                    {
                        poly.lastIndex = poly.AddPoint(GetWorldPoint(cam, e.mousePosition), insertPoint);
                    }

                    poly.handleOffset = Vector2.zero;
                    poly.isDraggingPoint = true;

                    poly.Refresh();
                    SceneView.RepaintAll();
                }
                break;

            case EventType.MouseUp:
                if (!poly.isDraggingPoint)
                {
                    break;
                }
                poly.isDraggingPoint = false;
                break;

            case EventType.MouseDrag:
                Undo.RecordObject(poly, "Move Point");

                if (snapEnabled)
                {
                    poly.SetPoint(poly.lastIndex, Round(GetWorldPoint(cam, e.mousePosition + poly.handleOffset), snapValue));
                }
                else
                {
                    poly.SetPoint(poly.lastIndex, GetWorldPoint(cam, e.mousePosition + poly.handleOffset));
                }

                poly.Refresh();
                SceneView.RepaintAll();
                break;
        }
    }

    #endregion

    #region Event

    void UndoRedoPerformed()
    {
        if (poly)
        {
            poly.isDraggingPoint = false;
            poly.Refresh();
        }

        SceneView.RepaintAll();
    }

    #endregion

    #region Setters

    Tool prevTool = (Tool)Tool.None;
    private void ToggleEditingEnabled()
    {
        poly.SetEditable(!poly.IsEditable);

        if (poly.IsEditable)
        {
            prevTool = Tools.current;
            Tools.current = Tool.None;
        }
        else
        {
            Tools.current = prevTool;
        }

        SceneView.RepaintAll();
    }

    public void SetSnapEnabled(bool enable)
    {
        snapEnabled = enable;
        EditorPrefs.SetBool("polyCollider_snapEnabled", snapEnabled);
    }

    public void SetSnapValue(float snapVal)
    {
        snapValue = snapVal;
        EditorPrefs.SetFloat("polyCollider_snapValue", snapValue);
    }

	public void setStartTime(float time) {
		startTime = time;
		polyStartEndDiff = endTime - startTime;
		poly._startTime = startTime;
		EditorUtility.SetDirty (poly.gameObject);
	}

	public void setEndTime(float time) {
		endTime = time;
		polyStartEndDiff = endTime - startTime;
		poly._endTime = endTime;
		EditorUtility.SetDirty (poly.gameObject);
	}

	public void setPositionBasedOnTimer(float time) {
		timeSlider = time;
		float timePercent = 0;
		if(polyStartEndDiff > 0.0f)
		{
			timePercent = (timeSlider / polyStartEndDiff) * polyStartEndDiff;
		}
		/*if(endTime > 0.0f) {
			timePercent = timeSlider / endTime;
		}*/
		for (int i = 0; i < poly._points.Count; ++i)
		{
			Vector2 start = poly.GetStartPosition (i);
			Vector2 end = poly.GetEndPosition(i);
			
			Vector2 dir = start + (end - start) * timePercent;
			poly.SetPoint(i, dir);
		}
	}

    #endregion

    #region Camera Conversions

    private Vector2 GetWorldPoint(Camera cam, Vector2 pos)
    {
      /*pos.y = Screen.height - pos.y - SCENEVIEW_HEADER;
        return cam.ScreenToWorldPoint(pos);*/

        Ray ray = Camera.current.ScreenPointToRay(new Vector3(pos.x, -pos.y + Camera.current.pixelHeight));
        Vector3 mousePos = ray.origin;

        return new Vector2(mousePos.x, mousePos.z);
    }

    #endregion
}
