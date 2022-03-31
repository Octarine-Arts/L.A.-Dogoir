using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class F
{
	public static float FastDistance (Vector3 start, Vector3 end) => (end - start).sqrMagnitude;
    public static bool Between (this float value, float min, float max) => value >= min && value <= max;
    public static bool Within (this float value, float target, float threshold) => value >= target - threshold / 2f && value <= target + threshold / 2f;
    public static float Wrap (this float value, float min = 0, float max = 1) => value - (max - min) * Mathf.Floor (value / (max - min));
    public static float Angle (this Vector2 vector) => Mathf.Atan2 (vector.x, vector.y) * Mathf.Rad2Deg;
    public static float Angle (this Vector3 vector) => Mathf.Atan2 (vector.x, vector.z) * Mathf.Rad2Deg;
    public static T Last<T> (this List<T> list) => list.Count > 0 ? list[list.Count - 1] : default(T);
    public static T Last<T> (this T[] array) => array.Length > 0 ? array[array.Length - 1] : default(T);
    public static void OffTheWeans (this Transform parent) { for (int i = 0; i < parent.childCount; i++) Object.Destroy (parent.GetChild (i).gameObject); }
}