using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Debug;

namespace Kars.Object
{

    class Grid<TGridObject>
    {
		public int Height { get; private set; }
        public int Width { get; private set; }
        public float Size { get; private set; }
        public Vector3 PositionToWorld { get; private set; }
		private TextMesh[,] textArray;
        public TGridObject[,] gridArray { get; private set; }

		public delegate void VoidFunc();
		public event VoidFunc ChangeValue;

		public Grid(int height, int width, float size, Vector3 positionToWorld, Func<TGridObject> createGridObject, bool isDebuging = false)
		{
			Height = height;
			Width = width;
			Size = size;
			PositionToWorld = positionToWorld;
			gridArray = new TGridObject[Height, Width];
			if (isDebuging) textArray = new TextMesh[Height, Width];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					TGridObject t = createGridObject();
					gridArray[y, x] = t;
					if (isDebuging)
					{
						textArray[y, x] = DebugUtilites.CreateWorldText(t.ToString(), null, GetWorldPosition(x, y) + new Vector3(Size, Size) * 0.5f, 40, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 0);
						UnityEngine.Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
						UnityEngine.Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
					}
				}
				if (isDebuging)
				{
					UnityEngine.Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
					UnityEngine.Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
				}
			}
		}
		public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * Size + PositionToWorld;
		public Vector3Int GetXY(Vector3 worldPosition)
		{
			int x, y;

			return GetXY(worldPosition, out x, out y);
		}
		public Vector3Int GetXY(Vector3 worldPosition, out int x, out int y)
		{
			x = Mathf.RoundToInt((worldPosition - PositionToWorld).x / Size - 0.5f);
			y = Mathf.RoundToInt((worldPosition - PositionToWorld).y / Size - 0.5f);

			return new Vector3Int(x, y);
		}
		public void SetValue(Vector3 worldPosition, TGridObject value)
		{
			int x, y;
			GetXY(worldPosition, out x, out y);
			SetValue(x, y, value);
		}
		public void SetValue(int x, int y, TGridObject value)
		{
			if (x >= 0 && x < Width && y >= 0 && y < Height)
			{
				gridArray[y, x] = value;
				//textMeshArray[pos.y, pos.x].text = gridArray[pos.y, pos.x].ToString();
				ChangeValue?.Invoke();
			}
		}
		public TGridObject GetValue(Vector3 worldPosition)
		{
			int x, y;
			GetXY(worldPosition, out x, out y);
			return GetValue(x, y);
		}
		public TGridObject GetValue(int x, int y)
		{
			if (x >= 0 && x < Width && y >= 0 && y < Height)
			{
				return gridArray[y, x];
			}
			else return default(TGridObject);
		}
	}

	static class KarsMesh
	{
		public static void CreateEmptyMeshArray(int quadCount, out Vector3[] vertices, out Vector2[] uv, out int[] triangles)
		{
			vertices = new Vector3[4 * quadCount];
			uv = new Vector2[4 * quadCount];
			triangles = new int[6 * quadCount];
		}
		public static void CreateEmptyMeshArray(int quadCount, out Vector3[] vertices, out Vector3[] normals, out Vector2[] uv, out int[] triangles)
		{
			normals = new Vector3[4 * quadCount];
			CreateEmptyMeshArray(quadCount, out vertices, out uv, out triangles);
		}
		public static void AddToMeshArray(Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles, int index, Vector3 worldPosition, float f, Vector3 tileSize, Vector3 v3, Vector2 imagePoint)
		{
			normals[index * 4 + 0] = Vector3.back;
			normals[index * 4 + 1] = Vector3.back;
			normals[index * 4 + 2] = Vector3.back;
			normals[index * 4 + 3] = Vector3.back;

			AddToMeshArray(vertices, uv, triangles, index, worldPosition, f, tileSize, v3, imagePoint);
		}
		public static void AddToMeshArray(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, Vector3 worldPosition, float f, Vector3 tileSize, Vector3 v3, Vector2 imagePoint)
		{
			tileSize *= .5f;
			vertices[index * 4 + 0] = new Vector3(-tileSize.x, -tileSize.y) + worldPosition;
			vertices[index * 4 + 1] = new Vector3(-tileSize.x, tileSize.y) + worldPosition;
			vertices[index * 4 + 2] = new Vector3(tileSize.x, tileSize.y) + worldPosition;
			vertices[index * 4 + 3] = new Vector3(tileSize.x, -tileSize.y) + worldPosition;

			uv[index * 4 + 0] = imagePoint + new Vector2(0.0f, 0.0f);
			uv[index * 4 + 1] = imagePoint + new Vector2(0.0f, 1.0f);
			uv[index * 4 + 2] = imagePoint + new Vector2(0.0f, 1.0f);
			uv[index * 4 + 3] = imagePoint + new Vector2(0.0f, 0.0f);

			triangles[index * 6 + 0] = index * 4 + 0;
			triangles[index * 6 + 1] = index * 4 + 1;
			triangles[index * 6 + 2] = index * 4 + 2;

			triangles[index * 6 + 3] = index * 4 + 0;
			triangles[index * 6 + 4] = index * 4 + 2;
			triangles[index * 6 + 5] = index * 4 + 3;
		}
	}
}

