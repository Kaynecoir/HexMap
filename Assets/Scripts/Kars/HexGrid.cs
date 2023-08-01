using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Debug;

namespace Kars.Object
{
    public class HexGrid<T> where T : IHexObject
    {
		public int Height { get; protected set; }
		public int Width { get; protected set; }
		public float Size { get; protected set; }
		public float Radius { get; protected set; }
		public float littleRadius { get; protected set; }
		public bool isVertical { get; private set; }
		public Vector3 PositionToCenter { get; protected set; }
		public Mesh gridMesh;
		protected TextMesh[,] textArray;
		public Hexagon<T>[,] gridArray { get; protected set; }

		public delegate void VoidFunc();
		public event VoidFunc ChangeValue;
		public HexGrid(int height, int width, float radius, Vector3 worldPosition, Vector3 positionToCenter, bool isVertical = false, bool isDebuging = false)
		{
			Height = height;
			Width = width;
			Radius = radius;
			littleRadius = radius * Mathf.Sin(Mathf.PI / 3);
			PositionToCenter = positionToCenter;
			this.isVertical = isVertical;
			gridArray = new Hexagon<T>[Height, Width];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{

					Hexagon<T> h = new Hexagon<T>(Radius, GetPositionFromCenter(x, y), isVertical: this.isVertical);
					h.SetCorner();
					h.SetGrid(x, y, this);
					if (x - 1 >= 0) h.AddNeigbourHex(gridArray[y, x - 1]);
					if (y - 1 >= 0) h.AddNeigbourHex(gridArray[y - 1, x]);
					if (isVertical)
					{
						if (y % 2 == 0 && x - 1 >= 0 && y - 1 >= 0) h.AddNeigbourHex(gridArray[y - 1, x - 1]);
						else if (y % 2 == 1 && x + 1 < Width) h.AddNeigbourHex(gridArray[y - 1, x + 1]);
					}
					else
					{
						if (x % 2 == 0 && x - 1 >= 0 && y - 1 >= 0) h.AddNeigbourHex(gridArray[y - 1, x - 1]);
						if (x % 2 == 0 && x + 1 < Width && y - 1 >= 0) h.AddNeigbourHex(gridArray[y - 1, x + 1]);
					}
					gridArray[y, x] = h;

				}
			}
		}
		public HexGrid(int height, int width, float radius, Vector3 worldPosition, Func<Hexagon<T>, T> createHexObject, Vector3 positionToCenter, bool isVertical = false, bool isDebuging = false)
		{
			Height = height;
			Width = width;
			Radius = radius;
			littleRadius = radius * Mathf.Sin(Mathf.PI / 3);
			PositionToCenter = positionToCenter;
			this.isVertical = isVertical;
			gridArray = new Hexagon<T>[Height, Width];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{

					Hexagon<T> h = new Hexagon<T>(Radius, GetPositionFromCenter(x, y), createHexObject, this.isVertical);

					h.SetCorner();
					h.SetGrid(x, y, this);
					if (x - 1 >= 0) h.AddNeigbourHex(gridArray[y, x - 1]);
					if (y - 1 >= 0) h.AddNeigbourHex(gridArray[y - 1, x]);
					if (isVertical)
					{
						if (y % 2 == 0 && x - 1 >= 0 && y - 1 >= 0) h.AddNeigbourHex(gridArray[y - 1, x - 1]);
						else if (y % 2 == 1 && x + 1 < Width) h.AddNeigbourHex(gridArray[y - 1, x + 1]);
					}
					else
					{
						if (x % 2 == 0 && x - 1 >= 0 && y - 1 >= 0) h.AddNeigbourHex(gridArray[y - 1, x - 1]);
						if (x % 2 == 0 && x + 1 < Width && y - 1 >= 0) h.AddNeigbourHex(gridArray[y - 1, x + 1]);
					}
					gridArray[y, x] = h;

				}
			}
		}
		public Hexagon<T> this[int y, int x]
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
					textArray[y, x] = DebugUtilites.CreateWorldText(gridArray[y, x].ToString(), null, GetPositionFromCenter(x, y) + new Vector3(Size, Size) * 0.5f, 40, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 0);
					UnityEngine.Debug.DrawLine(GetPositionFromCenter(x, y), GetPositionFromCenter(x, y + 1), Color.white, duration);
					UnityEngine.Debug.DrawLine(GetPositionFromCenter(x, y), GetPositionFromCenter(x + 1, y), Color.white, duration);

				}
				UnityEngine.Debug.DrawLine(GetPositionFromCenter(0, Height), GetPositionFromCenter(Width, Height), Color.white, duration);
				UnityEngine.Debug.DrawLine(GetPositionFromCenter(Width, 0), GetPositionFromCenter(Width, Height), Color.white, duration);
			}
		}
		public Vector3 GetPositionFromCenter(int x, int y)
		{
			Vector3 pos = Vector3.zero;
			if(isVertical)
			{
				pos = new Vector3((2 * x + 0) * MathF.Sqrt(3) / 2, y * 1.5f) * Radius + PositionToCenter;
				if (y % 2 == 1)
				{
					pos = new Vector3((2 * x + 1) * MathF.Sqrt(3) / 2, y * 1.5f) * Radius + PositionToCenter;
				}
			}
			else
			{
				pos = new Vector3(x * 1.5f, (2 * y + 0) * MathF.Sqrt(3) / 2) * Radius + PositionToCenter;
				if (x % 2 == 1)
				{
					pos = new Vector3(x * 1.5f, (2 * y + 1) * MathF.Sqrt(3) / 2) * Radius + PositionToCenter;
				}
			}

			return pos;
		}
		public bool inHexArea(int x, int y, Vector3 cursorPosition)
		{
			return gridArray[y, x].inHexArea(cursorPosition - gridArray[y, x].worldPosition);
		}
		public Vector3Int GetXY(Vector3 worldPosition)
		{
			return GetXY(worldPosition, out int x, out int y);
		}
		public Vector3Int GetXY(Vector3 CursorPosition, out int x, out int y)
		{
			x = 0; y = 0;
			Vector3 CursorToCenter = CursorPosition - PositionToCenter;
			UnityEngine.Debug.Log("cursor: " + CursorPosition);
			UnityEngine.Debug.Log("toCenter: " + PositionToCenter);
			UnityEngine.Debug.Log("CursorToCenter: " + CursorToCenter);


			if (isVertical)
			{
				y = Mathf.RoundToInt(CursorPosition.y * 2 / (3 * Radius) - 0.5f);
				x = Mathf.RoundToInt((CursorPosition.x / littleRadius - (y % 2) - 1.0f) / 2);
				
				UnityEngine.Debug.Log(x + " " + y);

				if (x >= 0 && x < Width && y >= 0 && y < Height)
				{
					UnityEngine.Debug.Log("O");
					if (y % 2 == 0)
					{
						if (inHexArea(x, y, CursorPosition))
						{
							return new Vector3Int(x, y);		//UnityEngine.Debug.Log($"{x}, {y}: {gridArray[y, x].worldPosition} <- {CursorPosition} = {(gridArray[y, x].worldPosition - CursorPosition).magnitude} -> {inHexArea(x, y, CursorPosition)}");
						}
						if (y - 1 >= 0 && x - 1 >= 0 && inHexArea(x - 1, y - 1, CursorPosition)) 
						{
							return new Vector3Int(--x, --y);  //UnityEngine.Debug.Log($"{x - 1}, {y - 1}: {gridArray[y - 1, x - 1].worldPosition} <- {CursorPosition} = {(gridArray[y - 1, x - 1].worldPosition - CursorPosition).magnitude} -> {inHexArea(x - 1, y - 1, CursorPosition)}");
						}
						if (y - 1 >= 0 && inHexArea(x, y - 1, CursorPosition))
						{
							return new Vector3Int(x, --y);  //UnityEngine.Debug.Log($"{x}, {y - 1}: {gridArray[y-1, x].worldPosition} <- {CursorPosition} = {(gridArray[y-1, x].worldPosition - CursorPosition).magnitude} -> {inHexArea(x, y-1, CursorPosition)}");
						}
					}
					else
					{
						if (inHexArea(x, y, CursorPosition)) 
						{
							return new Vector3Int(x, y);      // UnityEngine.Debug.Log($"{x}, {y}: {gridArray[y, x].worldPosition} <- {CursorPosition} = {(gridArray[y, x].worldPosition - CursorPosition).magnitude} -> {inHexArea(x, y, CursorPosition)}");
						}
						if (y - 1 >= 0 && x + 1 < Width && inHexArea(x + 1, y - 1, CursorPosition)) 
						{
							return new Vector3Int(++x, --y);   // UnityEngine.Debug.Log($"{x + 1}, {y - 1}: {gridArray[y - 1, x + 1].worldPosition} <- {CursorPosition} = {(gridArray[y - 1, x + 1].worldPosition - CursorPosition).magnitude} -> {inHexArea(x + 1, y - 1, CursorPosition)}");
						}
						if (y - 1 >= 0 && inHexArea(x, y - 1, CursorPosition)) 
						{
							return new Vector3Int(x, --y);      // UnityEngine.Debug.Log($"{x}, {y - 1}: {gridArray[y - 1, x].worldPosition} <- {CursorPosition} = {(gridArray[y - 1, x].worldPosition - CursorPosition).magnitude} -> {inHexArea(x, y - 1, CursorPosition)}");
						}
					}
				}
			}
			else
			{
				x = Mathf.RoundToInt(CursorToCenter.x * 2 / (3 * Radius) - 0.0f);
				y = Mathf.RoundToInt((CursorToCenter.y / littleRadius - (x % 2) - 0.0f) / 2);

				UnityEngine.Debug.Log(x + " " + y);

				if (x >= 0 && x < Width && y >= 0 && y < Height)
				{
					UnityEngine.Debug.Log("X");

					if (x % 2 == 0)
					{
						if (inHexArea(x, y, CursorPosition))
						{
							return new Vector3Int(x, y);        //UnityEngine.Debug.Log($"{x}, {y}: {gridArray[y, x].worldPosition} <- {CursorPosition} = {(gridArray[y, x].worldPosition - CursorPosition).magnitude} -> {inHexArea(x, y, CursorPosition)}");
						}
						if (x - 1 >= 0 && y - 1 >= 0 && inHexArea(x - 1, y - 1, CursorPosition))
						{
							return new Vector3Int(--x, --y);  //UnityEngine.Debug.Log($"{x - 1}, {y - 1}: {gridArray[y - 1, x - 1].worldPosition} <- {CursorPosition} = {(gridArray[y - 1, x - 1].worldPosition - CursorPosition).magnitude} -> {inHexArea(x - 1, y - 1, CursorPosition)}");
						}
						if (x - 1 >= 0 && inHexArea(x - 1, y, CursorPosition))
						{
							return new Vector3Int(x, --y);  //UnityEngine.Debug.Log($"{x}, {y - 1}: {gridArray[y-1, x].worldPosition} <- {CursorPosition} = {(gridArray[y-1, x].worldPosition - CursorPosition).magnitude} -> {inHexArea(x, y-1, CursorPosition)}");
						}
					}
					else
					{
						if (inHexArea(x, y, CursorPosition))
						{
							return new Vector3Int(x, y);        //UnityEngine.Debug.Log($"{x}, {y}: {gridArray[y, x].worldPosition} <- {CursorPosition} = {(gridArray[y, x].worldPosition - CursorPosition).magnitude} -> {inHexArea(x, y, CursorPosition)}");
						}
						if (x - 1 >= 0 && y + 1 < Height && inHexArea(x - 1, y + 1, CursorPosition))
						{
							return new Vector3Int(--x, ++y);  //UnityEngine.Debug.Log($"{x - 1}, {y - 1}: {gridArray[y - 1, x - 1].worldPosition} <- {CursorPosition} = {(gridArray[y - 1, x - 1].worldPosition - CursorPosition).magnitude} -> {inHexArea(x - 1, y - 1, CursorPosition)}");
						}
						if (x - 1 >= 0 && inHexArea(x - 1, y, CursorPosition))
						{
							return new Vector3Int(x, --y);  //UnityEngine.Debug.Log($"{x}, {y - 1}: {gridArray[y-1, x].worldPosition} <- {CursorPosition} = {(gridArray[y-1, x].worldPosition - CursorPosition).magnitude} -> {inHexArea(x, y-1, CursorPosition)}");
						}
					}
				}
			}
			x = 0;	y = 0;
			return new Vector3Int(x, y);
		}
		public void SetValue(Vector3 worldPosition, Hexagon<T> hex)
		{
			int x, y;
			GetXY(worldPosition, out x, out y);
			SetValue(x, y, hex);
		}
		public void SetValue(int x, int y, Hexagon<T> hex)
		{
			if (x >= 0 && x < Width && y >= 0 && y < Height)
			{
				gridArray[y, x] = hex;
				//textMeshArray[pos.y, pos.x].text = gridArray[pos.y, pos.x].ToString();
				ChangeValue?.Invoke();
			}
		}
		public Hexagon<T> GetValue(Vector3 worldPosition)
		{
			GetXY(worldPosition, out int x, out int y);
			return GetValue(x, y);
		}
		public Hexagon<T> GetValue(int x, int y)
		{
			if (x >= 0 && x < Width && y >= 0 && y < Height)
			{
				return gridArray[y, x];
			}
			else return default(Hexagon<T>);
		}
		public Mesh CreateMeshArray()
		{
			gridMesh = new Mesh();
			gridMesh.name = "HexGrid";
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
			gridMesh.vertices = vertices;
			gridMesh.normals = normals;
			gridMesh.uv = uv;
			gridMesh.triangles = triangles;

			return gridMesh;
		}
		public Mesh CreateMeshArray(Vector2 uvPoint)
		{
			gridMesh = new Mesh();
			gridMesh.name = "HexGrid";
			Vector3[] vertices = new Vector3[(Width * Height) * 7];
			Vector3[] normals = new Vector3[(Width * Height) * 7];
			Vector2[] uv = new Vector2[(Width * Height) * 7];
			int[] triangles = new int[(Width * Height) * 18];

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					int index = y * Width + x;
					AddHexMeshToArray(GetValue(x, y), vertices, normals, uv, triangles, index, uvPoint);
				}
			}
			gridMesh.vertices = vertices;
			gridMesh.normals = normals;
			gridMesh.uv = uv;
			gridMesh.triangles = triangles;

			return gridMesh;
		}

		public void AddHexMeshToArray(Hexagon<T> hex, Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles, int index)
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
		public void AddHexMeshToArray(Hexagon<T> hex, Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles, int index, Vector2 uvPoint)
		{
			for (int i = 0; i < 6; i++)
			{
				vertices[index * 7 + i] = hex.corner[i];
				normals[index * 7 + i] = Vector3.back;
				uv[index * 7 + i] = uvPoint;
			}
			vertices[index * 7 + 6] = hex.worldPosition;
			normals[index * 7 + 6] = Vector3.back;
			uv[index * 7 + 6] = uvPoint;

			for (int i = 0; i < 6; i++)
			{
				triangles[index * 18 + i * 3 + 0] = (index * 7 + 6);
				triangles[index * 18 + i * 3 + 1] = ((i == 5) ? (index * 7 + 0) : (index * 7 + i + 1));
				triangles[index * 18 + i * 3 + 2] = (index * 7 + i);
			}
		}
		public void ClearHexMeshArray()
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					int index = y * Width + x;
					for (int i = 0; i < 6; i++)
					{
						gridMesh.uv[index * 7 + i] = (GetValue(x, y).corner[i] - GetValue(x, y).worldPosition) / Radius / 2 + new Vector3(0.5f, 0.5f);
					}
					gridMesh.uv[index * 7 + 6] = new Vector2(0.5f, 0.5f);
				}
			}
		}
	}
}

