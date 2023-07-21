using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using CodeMonkey.Utils;

public class WorldText : MonoBehaviour
{
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
	public static Vector3 GetMouseWorldPosition()
	{
		return GetMouseWorldPosition(Input.mousePosition, Camera.main);
	}
	public static Vector3 GetMouseWorldPosition(Transform transform)
	{
		return GetMouseWorldPosition(transform, Input.mousePosition, Camera.main);
	}
	public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
	{
		Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
		return worldPosition;
	}
	public static Vector3 GetMouseWorldPosition(Transform transform, Vector3 screenPosition, Camera worldCamera)
	{
		Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition) - transform.position;
		Debug.Log(worldCamera.ScreenToViewportPoint(screenPosition) - transform.position);
		return worldPosition;
	}
	public static void CreateEmptyMeshArray(int quadCount, out Vector3[] vertices, out Vector3[] normals, out Vector2[] uv, out int[] triangles)
	{
		vertices = new Vector3[4 * quadCount];
		normals = new Vector3[4 * quadCount];
		uv = new Vector2[4 * quadCount];
		triangles = new int[6 * quadCount];
	}
	public static void AddToMeshArray(Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles, int index, Vector3 worldPosition, float f, Vector3 tileSize, Vector3 v3, Vector2 imagePoint)
	{
		//tileSize *= .5f;
		//vertices[index * 4 + 0] = new Vector3(-tileSize.x, -tileSize.y);
		//vertices[index * 4 + 1] = new Vector3(-tileSize.x,  tileSize.y);
		//vertices[index * 4 + 2] = new Vector3( tileSize.x,  tileSize.y);
		//vertices[index * 4 + 3] = new Vector3( tileSize.x, -tileSize.y);
		tileSize *= .5f;
		vertices[index * 4 + 0] = new Vector3(-tileSize.x, -tileSize.y) + worldPosition;
		vertices[index * 4 + 1] = new Vector3(-tileSize.x,  tileSize.y) + worldPosition;
		vertices[index * 4 + 2] = new Vector3( tileSize.x,  tileSize.y)	+ worldPosition;
		vertices[index * 4 + 3] = new Vector3( tileSize.x, -tileSize.y) + worldPosition;

		normals[index * 4 + 0] = Vector3.back;
		normals[index * 4 + 1] = Vector3.back;
		normals[index * 4 + 2] = Vector3.back;
		normals[index * 4 + 3] = Vector3.back;

		uv[index * 4 + 0] = imagePoint + new Vector2(0.01f, 0.01f);
		uv[index * 4 + 1] = imagePoint + new Vector2(0.01f, 0.02f);
		uv[index * 4 + 2] = imagePoint + new Vector2(0.02f, 0.02f);
		uv[index * 4 + 3] = imagePoint + new Vector2(0.02f, 0.01f);

		triangles[index * 6 + 0] = index * 4 + 0;
		triangles[index * 6 + 1] = index * 4 + 1;
		triangles[index * 6 + 2] = index * 4 + 2;

		triangles[index * 6 + 3] = index * 4 + 0;
		triangles[index * 6 + 4] = index * 4 + 2;
		triangles[index * 6 + 5] = index * 4 + 3;
	}
	public static void AddToMeshArray(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, Vector3 worldPosition, float f, Vector3 tileSize, Vector3 v3, Vector2 imagePoint)
	{
		//tileSize *= .5f;
		//vertices[index * 4 + 0] = new Vector3(-tileSize.x, -tileSize.y);
		//vertices[index * 4 + 1] = new Vector3(-tileSize.x,  tileSize.y);
		//vertices[index * 4 + 2] = new Vector3( tileSize.x,  tileSize.y);
		//vertices[index * 4 + 3] = new Vector3( tileSize.x, -tileSize.y);
		tileSize *= .5f;
		vertices[index * 4 + 0] = new Vector3(-tileSize.x, -tileSize.y) + worldPosition;
		vertices[index * 4 + 1] = new Vector3(-tileSize.x, tileSize.y) + worldPosition;
		vertices[index * 4 + 2] = new Vector3(tileSize.x, tileSize.y) + worldPosition;
		vertices[index * 4 + 3] = new Vector3(tileSize.x, -tileSize.y) + worldPosition;

		uv[index * 4 + 0] = imagePoint + new Vector2(0.01f, 0.01f);
		uv[index * 4 + 1] = imagePoint;
		uv[index * 4 + 2] = imagePoint;
		uv[index * 4 + 3] = imagePoint;

		triangles[index * 6 + 0] = index * 4 + 0;
		triangles[index * 6 + 1] = index * 4 + 1;
		triangles[index * 6 + 2] = index * 4 + 2;

		triangles[index * 6 + 3] = index * 4 + 0;
		triangles[index * 6 + 4] = index * 4 + 2;
		triangles[index * 6 + 5] = index * 4 + 3;
	}
	public static Mesh CreateTileMesh(int height, int width, float tileSize)
	{
		Mesh mesh = new Mesh();
		mesh.name = "MyName";

		Vector3[] vertices = new Vector3[4 * (height * width)];
		Vector2[] uv = new Vector2[4 * (height * width)];
		int[] triangles = new int[6 * (height * width)];

		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				int index = y * height + x;
				vertices[index * 4 + 0] = new Vector3(tileSize * x		, tileSize * y);
				vertices[index * 4 + 1] = new Vector3(tileSize * x		, tileSize * (y+1));
				vertices[index * 4 + 2] = new Vector3(tileSize * (x+1)	, tileSize * (y+1));
				vertices[index * 4 + 3] = new Vector3(tileSize * (x+1)	, tileSize * y);

				uv[index * 4 + 0] = new Vector2(0, 0);
				uv[index * 4 + 1] = new Vector2(0, 1);
				uv[index * 4 + 2] = new Vector2(1, 1);
				uv[index * 4 + 3] = new Vector2(1, 0);

				triangles[index * 6 + 0] = index * 4 + 0;
				triangles[index * 6 + 1] = index * 4 + 1;
				triangles[index * 6 + 2] = index * 4 + 2;

				triangles[index * 6 + 3] = index * 4 + 0;
				triangles[index * 6 + 4] = index * 4 + 2;
				triangles[index * 6 + 5] = index * 4 + 3;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;

		return mesh;
	}
}
