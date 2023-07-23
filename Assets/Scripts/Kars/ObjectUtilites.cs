using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kars.Object
{

    class Grid<TGridObject>
    {

		public int Height { get; private set; }
        public int Width { get; private set; }
        public float Size { get; private set; }
        public Vector3 PositionToWorld { get; private set; }
        public TGridObject[,] gridArray { get; private set; }

		public delegate void VoidFunc();
		public delegate TGridObject CreateGridObject<TGridObject>();
		public event VoidFunc ChangeValue;

		public Grid(int height, int width, float size, Vector3 positionToWorld, Func<TGridObject> createGridObject)
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
					UnityEngine.Debug.Log(gridArray[y, x]);
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
}

