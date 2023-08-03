using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Debug;

namespace Kars
{
	namespace Object
	{
		public class Grid<TGridObject>
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
	}
}
