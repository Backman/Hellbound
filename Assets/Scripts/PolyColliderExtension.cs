using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

public static class PolyColliderExtension
{

    #region Conversion

    public static List<Vector2> ToVector2(this Vector3[] v)
    {
        List<Vector2> p = new List<Vector2>();
        for (int i = 0; i < v.Length; i++)
            p.Add(v[i]);
        return p;
    }

    public static Vector3 ToVector3(this Vector2 v2)
    {
        return new Vector3(v2.x, 0.0f, v2.y);
    }

    public static Vector3 ToVector3(this Vector2 v2, float y)
    {
        return new Vector3(v2.x, y, v2.y);
    }

    public static Vector3[] ToVector3(this List<Vector2> v2, float y)
    {
        Vector3[] v = new Vector3[v2.Count];
        for (int i = 0; i < v.Length; i++)
            v[i] = new Vector3(v2[i].x, y, v2[i].y);
        return v;
    }

    public static Vector3[] ToWorldSpace(this Transform t, Vector3[] v)
    {
        Vector3[] nv = new Vector3[v.Length];
        for (int i = 0; i < nv.Length; i++)
            nv[i] = t.TransformPoint(v[i]);
        return nv;
    }

    public static Vector3[] ToWorldSpace(this Transform t, List<Vector3> v)
    {
        Vector3[] nv = new Vector3[v.Count];
        for (int i = 0; i < nv.Length; i++)
            nv[i] = t.TransformPoint(v[i]);
        return nv;
    }
    #endregion
}