using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	Grid grid;

	private void Start()
	{
		grid = new Grid(2, 3, 10f, new Vector3(10, -20));
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			grid.SetValue(WorldText.GetMouseWorldPosition(), 12);
		}
		if (Input.GetMouseButtonDown(1))
		{
			Debug.Log(grid.GetValue(WorldText.GetMouseWorldPosition()));
		}
	}

	private class HeatMapVisual
	{
		private Grid grid;
		public HeatMapVisual(Grid grid)
		{
			this.grid = grid;

			//Vector3[] vertices;
			//Vector2[] uv;
			//int[] triangles;

			

			for(int y = 0; y < grid.GetHeight(); y++)
			{
				for(int x = 0; x < grid.GetWidth(); x++)
				{
					
				}
			}
		}
	}
}

