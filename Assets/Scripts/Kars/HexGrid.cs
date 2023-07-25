using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Debug;

namespace Kars.Object
{
    class HexGrid
    {
		public int Height { get; protected set; }
		public int Width { get; protected set; }
		public float Size { get; protected set; }
		public float Radius { get; protected set; }
		public Vector3 PositionToWorld { get; protected set; }
		protected TextMesh[,] textArray;
		public Hex[,] gridArray { get; protected set; }

		public delegate void VoidFunc();
		public event VoidFunc ChangeValue;
		public HexGrid(int height, int width, float size, Vector3 positionToWorld, Func<HexGrid, int, int, Hex> createGridObject, bool isVertical = false, bool isDebuging = false)
		{
			Height = height;
			Width = width;
			Size = size;
			PositionToWorld = positionToWorld;
			gridArray = new Hex[Height, Width];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Hex h = new Hex(Radius, GetWorldPosition(x, y));
					UnityEngine.Debug.Log(">>> " + x + " " + y + ": " + h.worldPosition);
					h.SetCorner();
					gridArray[y, x] = h;

				}
			}
		}
		public Hex this[int y, int x]
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
		public Vector3 GetWorldPosition(int x, int y)
		{
			Vector3 pos;
			float step;
			if (x % 2 == 0)
			{
				step = Radius / 2 * 3;
				pos = new Vector3(x * Size, y * Size);
			}
			return new Vector3(x, y) * Size + PositionToWorld;
		}
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
		public void SetValue(Vector3 worldPosition, Hex value)
		{
			int x, y;
			GetXY(worldPosition, out x, out y);
			SetValue(x, y, value);
		}
		public void SetValue(int x, int y, Hex value)
		{
			if (x >= 0 && x < Width && y >= 0 && y < Height)
			{
				gridArray[y, x] = value;
				//textMeshArray[pos.y, pos.x].text = gridArray[pos.y, pos.x].ToString();
				ChangeValue?.Invoke();
			}
		}
		public Hex GetValue(Vector3 worldPosition)
		{
			int x, y;
			GetXY(worldPosition, out x, out y);
			return GetValue(x, y);
		}
		public Hex GetValue(int x, int y)
		{
			if (x >= 0 && x < Width && y >= 0 && y < Height)
			{
				return gridArray[y, x];
			}
			else return default(Hex);
		}
		public Mesh CreateMeshArray()
		{
			Mesh mesh = new Mesh();
			mesh.name = "HexGrid";
			Vector3[] vertices = new Vector3[(Width * Height) * 7];
			Vector3[] normals = new Vector3[(Width * Height) * 7];
			Vector2[] uv = new Vector2[(Width * Height) * 7];
			int[] triangles = new int[(Width * Height) * 18];

			for(int y = 0; y < Height; y++)
			{
				for(int x = 0; x < Width; x++)
				{
					int index = y * Width + x;
					AddHexMeshToArray(GetValue(x, y), vertices, normals, uv, triangles, index);
				}
			}
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			return mesh;
		}
		public void AddHexMeshToArray(Hex hex, Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles, int index)
		{
			for (int i = 0; i < 6; i++)
			{
				vertices[index * 7 + i] = hex.corner[i];// + worldPosition;
														//UnityEngine.Debug.Log(vertices[index * 7 + i]);
				normals[index * 7 + i] = Vector3.back;
				uv[index * 7 + i] = (hex.corner[i] - hex.worldPosition) / hex.radius / 2 + new Vector3(0.5f, 0.5f);
			}
			vertices[index * 7 + 6] = hex.worldPosition;// + worldPosition;
			normals[index * 7 + 6] = Vector3.back;
			uv[index * 7 + 6] = new Vector2(0.5f, 0.5f);

			for (int i = 0; i < 6; i++)
			{
				triangles[index * 18 + i * 3 + 0] = (index * 7 + 6);
				triangles[index * 18 + i * 3 + 1] = ((i == 5) ? (index * 7 + 0) : (index * 7 + i + 1));
				triangles[index * 18 + i * 3 + 2] = (index * 7 + i);
			}
		}
	}
}

