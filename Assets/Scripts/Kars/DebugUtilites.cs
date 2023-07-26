using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kars.Debug
{
	public static class DebugUtilites
	{
		public static Vector3 GetMouseWorldPosition() => GetMouseWorldPosition(Vector3.zero, Input.mousePosition, Camera.main);
		public static Vector3 GetMouseWorldPosition(Vector3 nullCoordinate) => GetMouseWorldPosition(nullCoordinate, Input.mousePosition, Camera.main);
		public static Vector3 GetMouseWorldPosition(Vector3 nullCoordinate, Vector3 screenPosition, Camera worldCamera)
		{
			Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition) - nullCoordinate;
			return worldPosition;
		}
		public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int soringOrder)
		{
			GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
			Transform transform = gameObject.transform;
			transform.SetParent(parent, false);
			transform.localPosition = localPosition;
			TextMesh textObj = gameObject.GetComponent<TextMesh>();
			textObj.anchor = textAnchor;
			textObj.alignment = textAlignment;
			textObj.text = text;
			textObj.fontSize = fontSize;
			textObj.color = color;
			textObj.GetComponent<MeshRenderer>().sortingOrder = soringOrder;

			return textObj;
		}
		public static void RotateSysCoord(float angel_deg, ref float x, ref float y)
		{
			x *= Mathf.Cos(angel_deg / 180 * Mathf.PI);
			y *= Mathf.Sin(angel_deg / 180 * Mathf.PI);
		}
	}
}

