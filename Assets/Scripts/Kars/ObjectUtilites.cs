using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Debug;

namespace Kars.Object
{
    class Grid<TGridObject>
    {
		public int Height { get; protected set; }
        public int Width { get; protected set; }
        public float Size { get; protected set; }
        public Vector3 PositionToWorld { get; protected set; }
		protected TextMesh[,] textArray;
        public TGridObject[,] gridArray { get; protected set; }

		public delegate void VoidFunc();
		public event VoidFunc ChangeValue;

		public Grid()
		{

		}
		public Grid(int height, int width, float size, Vector3 positionToWorld, Func<TGridObject> createGridObject, bool isDebuging = false)
		{
			Height = height;
			Width = width;
			Size = size;
			PositionToWorld = positionToWorld;
			gridArray = new TGridObject[Height, Width];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					TGridObject t = createGridObject();
					gridArray[y, x] = t;
				}
			}
		}
		public Grid(int height, int width, float size, Vector3 positionToWorld, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool isDebuging = false)
		{
			Height = height;
			Width = width;
			Size = size;
			PositionToWorld = positionToWorld;
			gridArray = new TGridObject[Height, Width];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					TGridObject t = createGridObject(this, y, x);
					gridArray[y, x] = t;

				}
			}
		}

		public TGridObject this[int y, int x]
		{
			get
			{
				return gridArray[y, x];
			}
			private set
			{
				gridArray[y, x] = value;
			}
		}
		public void SeeDebug(float duration = 100f)
		{
			textArray = new TextMesh[Height, Width];

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					textArray[y, x] = DebugUtilites.CreateWorldText(gridArray[y, x].ToString(), null, GetWorldPosition(x, y) + new Vector3(Size, Size) * 0.5f, 40, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 0);
					UnityEngine.Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, duration);
					UnityEngine.Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, duration);

				}
				UnityEngine.Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, duration);
				UnityEngine.Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, duration);
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
			x = x < Width && x >= 0 ? x : 0;
			y = y < Height && y >= 0 ? y : 0;

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

	class Hex
	{
		public HexGrid hexGrid;
		private int gridHeight, gridWidth;
		public Vector3 worldPosition;
		public Vector3[] corner { get; private set; }
		public List<Hex> neigbourHex;
		public int positionX { get; private set; }
		public int positionY { get; private set; }
		public float radius { get; private set; }
		public float littleRadius { get; protected set; }
		public float height { get; private set; }	// for 3D
		public bool isVertical { get; private set; }

		public Hex(float radius, bool isVertical = false)
		{
			this.radius = radius;
			this.littleRadius = radius * Mathf.Sin(Mathf.PI / 3);
			this.worldPosition = Vector3.zero;
			this.isVertical = isVertical;
			corner = new Vector3[6];

			//SetCorner();
		}
		public Hex(float radius, Vector3 worldPosition, bool isVertical = false)
		{
			this.radius = radius;
			this.littleRadius = radius * Mathf.Sin(Mathf.PI / 3);
			this.worldPosition = worldPosition;
			this.isVertical = isVertical;
			corner = new Vector3[6];

			SetCorner();
		}

		public void SetCorner()
		{
			for (int i = 0; i < 6; i++)
			{
				float angel_del = 60 * i + (isVertical ? 90 : 0);
				float angel_rad = angel_del / 180 * MathF.PI;
				corner[i] = new Vector3(Mathf.Cos(angel_rad), Mathf.Sin(angel_rad)) * radius + worldPosition;
			}
		}
		public bool inTrianArea(float x, float y)
		{
			if (x < radius / 2 && x >= 0) return Mathf.Tan(Mathf.PI / 3) > x / y;
			if (x >= radius / 2 && x <= radius) return Mathf.Tan(Mathf.PI / 3) > (radius - x) / y;
			return false;
		}
		public bool inHexArea(Vector3 pos)
		{
			return inHexArea(pos.x, pos.y);
		}
		public bool inHexArea(float x, float y)
		{
			float angel_del = 0;

			for (int i = 0; i < 6; i++)
			{
				DebugUtilites.RotateSysCoord(angel_del, ref x, ref y);
				if (inTrianArea(x, y)) return true;
				angel_del += 60;
			}
			return false;
		}

		public void DrawHex()
		{
			for(int i = 0; i < 6; i++)
			{
			}
		}
		
		public Mesh CreateHexMesh()
		{
			Mesh mesh = new Mesh();
			mesh.name = "Hex";
			Vector3[] vertices = new Vector3[7];
			Vector3[] normals = new Vector3[7];
			Vector2[] uv = new Vector2[7];
			for(int i = 0; i < 6; i++)
			{
				vertices[i] = corner[i];
				normals[i] = Vector3.back;
				uv[i] = corner[i] / radius / 2 + new Vector3(0.5f, 0.5f);
			}
			vertices[6] = new Vector3(0, 0);
			normals[6] = Vector3.back;
			uv[6] = new Vector2(0.5f, 0.5f);

			int[] triangles = new int[18];
			for(int i = 0; i < 6; i++)
			{
				triangles[i * 3 + 0] = 6;
				triangles[i * 3 + 1] = i == 5 ? 0 : i+1;
				triangles[i * 3 + 2] = i;
			}

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			return mesh;
		}
	}
}

