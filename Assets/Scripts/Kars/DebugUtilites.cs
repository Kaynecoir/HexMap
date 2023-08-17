using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karsss.Debug
{
	public static class DebugUtilites
	{
		public static Vector3 GetMouseWorldPosition() => GetMouseWorldPosition(Vector3.zero, Input.mousePosition, Camera.main);
		public static Vector3 GetMouseWorldPosition(Vector3 nullCoordinate) => GetMouseWorldPosition(nullCoordinate, Input.mousePosition, Camera.main);
		public static Vector3 GetMouseWorldPosition(Vector3 nullCoordinate, Vector3 screenPosition, Camera worldCamera)
		{
			Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition) - nullCoordinate;
			worldPosition = new Vector3(worldPosition.x, worldPosition.y);
			return worldPosition;
		}

		public static Vector3 GetMouseScreenPosition() => GetMouseScreenPosition(Vector3.zero, Input.mousePosition, Camera.main);
		public static Vector3 GetMouseScreenPosition(Vector3 nullCoordinate) => GetMouseScreenPosition(nullCoordinate, Input.mousePosition, Camera.main);
		public static Vector3 GetMouseScreenPosition(Vector3 nullCoordinate, Vector3 worldPosition, Camera worldCamera)
		{
			Vector3 screenPosition = worldCamera.ScreenToWorldPoint(worldPosition) - nullCoordinate;
			UnityEngine.Debug.Log("worldPosition " + worldPosition);
			screenPosition = new Vector3(screenPosition.x, screenPosition.y);
			return screenPosition;
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
		public static void RotateSysCoord(float new_angel_deg, ref float x, ref float y)
		{
			float new_angel_rad = new_angel_deg / 180 * Mathf.PI;
			float tx = x, ty = y;

			x = tx * Mathf.Cos(new_angel_rad) + ty * Mathf.Sin(new_angel_rad);
			y = ty * Mathf.Cos(new_angel_rad) - tx * Mathf.Sin(new_angel_rad);

		}
		public static Mesh TriangleMesh(Vector3 worldPosition, float side)
		{
			Mesh mesh = new Mesh();
			mesh.name = "Triangle";
			Vector3[] vertices = new Vector3[3];
			Vector3[] normals = new Vector3[3];
			Vector2[] uv = new Vector2[3];
			vertices[0] = worldPosition;
			vertices[1] = worldPosition + new Vector3(Mathf.Cos(Mathf.PI / 3) * side, Mathf.Sin(Mathf.PI / 3) * side);
			vertices[2] = worldPosition + new Vector3(side, 0);
			normals[0] = Vector3.back;
			normals[1] = Vector3.back;
			normals[2] = Vector3.back;
			uv[0] = worldPosition;
			uv[1] = worldPosition + new Vector3(side, Mathf.Sin(Mathf.PI / 3) * side);
			uv[2] = worldPosition + new Vector3(Mathf.Cos(Mathf.PI / 3) * side, side);

			int[] triangles = new int[3];
			triangles[0] = 0;
			triangles[1] = 1;
			triangles[2] = 2;

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			return mesh;
		}
		public static bool inTrianArea(Vector3 pos, float radius)
		{
			return inTrianArea(pos.x, pos.y, radius);
		}
		public static bool inTrianArea(float x, float y, float radius)
		{
			if (y < 0) return false;
			if (x < radius / 2 && x >= 0) return Mathf.Tan(Mathf.PI / 3) > y / x;
			if (x >= radius / 2 && x <= radius) return Mathf.Tan(Mathf.PI / 3) > y / (radius - x);
			return false;
		}
	}
}

