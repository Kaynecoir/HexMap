using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
	private int height, width;
	private float cellSize;
	private int[,] gridArray;
	private TextMesh[,] textMeshArray;
	private Vector3 originPosition;

	public delegate void VoidFunc();
	public event VoidFunc ChangeValue;

	public Grid(int height, int width, float cellSize, Vector3 originPosition)
	{
		this.height = height;
		this.width = width;
		this.cellSize = cellSize;
		this.originPosition = originPosition;

		gridArray = new int[height, width];
		textMeshArray = new TextMesh[height, width];

		for(int y = 0; y < height; y ++)
		{
			for(int x = 0; x < width; x++)
			{
				//Debug.Log("X: " + x + " y: " + y + " = " + gridArray[y, x]);
				textMeshArray[y, x] = WorldText.CreateWorldText(gridArray[y, x].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 0);
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white, 100f);
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100f);
			}
		}
		Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
		Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

	}

	public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * cellSize + originPosition;
	public int GetWidth() => width;
	public int GetHeight() => height;
	public float GetCellSize() => cellSize;
	public void SetValue(int x, int y, int value)
	{
		if (x >= 0 && x < width && y >= 0 && y < height)
		{
			gridArray[y, x] = value;
			textMeshArray[y, x].text = gridArray[y, x].ToString();
		}
	}

	public Vector3Int GetXY(Vector3 worldPosition)
	{
		int x = Mathf.RoundToInt((worldPosition - originPosition).x / cellSize - 0.5f);
		int y = Mathf.RoundToInt((worldPosition - originPosition).y / cellSize - 0.5f);

		return new Vector3Int(x, y);
	}

	public void SetValue(Vector3 worldPosition, int value)
	{
		Vector3Int pos = GetXY(worldPosition);
		if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
		{
			gridArray[pos.y, pos.x] = value;
			textMeshArray[pos.y, pos.x].text = gridArray[pos.y, pos.x].ToString();
			ChangeValue?.Invoke();
		}
	}

	public int GetValue(Vector3 worldPosition)
	{
		Vector3Int pos = GetXY(worldPosition);
		if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
		{
			return gridArray[pos.y, pos.x];
		}
		else return 0;
	}

	public int GetValue(int x, int y)
	{
		Vector3Int pos = new Vector3Int(x, y);
		if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
		{
			return gridArray[pos.y, pos.x];
		}
		else return 0;
	}
}
