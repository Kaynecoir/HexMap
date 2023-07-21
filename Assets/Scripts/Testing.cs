using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	Grid grid;
	Material material;
	MeshFilter meshFilter;
	public int height, width;

	private void Start()
	{
		grid = new Grid(height, width, 10f, new Vector3(0, 0));
		meshFilter = GetComponent<MeshFilter>();
		HeatMapVisual heatMapVisual = new HeatMapVisual(grid, meshFilter);
		//GetComponent<MeshFilter>().mesh = WorldText.CreateTileMesh(grid.GetHeight(), grid.GetWidth(), grid.GetCellSize());
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			grid.SetValue(WorldText.GetMouseWorldPosition(), grid.GetValue(WorldText.GetMouseWorldPosition()) + 1);
		}
		if (Input.GetMouseButtonDown(1))
		{
			Debug.Log(grid.GetValue(WorldText.GetMouseWorldPosition()));
		}
	}

	private class HeatMapVisual
	{
		private Grid grid;
		MeshFilter meshFilter;
		public HeatMapVisual(Grid grid, MeshFilter meshFilter)
		{
			this.grid = grid;
			this.grid.ChangeValue += UpdateHeatMapVisual;
			this.meshFilter = meshFilter;

			Vector3[] vertices;
			Vector3[] normals;
			Vector2[] uv;
			int[] triangles;

			WorldText.CreateEmptyMeshArray(grid.GetHeight() * grid.GetWidth(), out vertices, out normals, out uv, out triangles);
			Vector3 baseSize = new Vector3(1, 1) * grid.GetCellSize();

			for (int y = 0; y < grid.GetHeight(); y++)
			{
				for(int x = 0; x < grid.GetWidth(); x++)
				{
					Debug.Log("y: " + y + " x: " + x);
					int index = y * grid.GetWidth() + x;
					Debug.Log(grid.GetWorldPosition(x, y));
					WorldText.AddToMeshArray(vertices, normals, uv, triangles, index, grid.GetWorldPosition(x, y) + baseSize * 0.5f, 0f, baseSize, Vector3.zero, Vector2.zero);
				}
			}
			Mesh mesh = new Mesh();
			mesh.name = "MyMesh";
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			meshFilter.mesh = mesh;
		}

		public void UpdateHeatMapVisual()
		{
			Vector3[] vertices;
			Vector3[] normals;
			Vector2[] uv;
			int[] triangles;

			WorldText.CreateEmptyMeshArray(grid.GetHeight() * grid.GetWidth(), out vertices, out normals, out uv, out triangles);
			Vector3 baseSize = new Vector3(1, 1) * grid.GetCellSize();
			int maxGridValue = 10;

			for (int y = 0; y < grid.GetHeight(); y++)
			{
				for (int x = 0; x < grid.GetWidth(); x++)
				{
					int index = y * grid.GetWidth() + x;
					int gridValue = grid.GetValue(x, y);
					if(gridValue >= maxGridValue)
					{
						grid.SetValue(x, y, maxGridValue-1);
						gridValue = maxGridValue - 1;
					}
					float gridValueNormalized = Mathf.Clamp01((float)gridValue / maxGridValue);
					Vector2 gridCellUV = new Vector2(gridValueNormalized, 0f);
					WorldText.AddToMeshArray(vertices, normals, uv, triangles, index, grid.GetWorldPosition(x, y) + baseSize * 0.5f, 0f, baseSize, Vector3.zero, gridCellUV);
				}
			}
			Mesh mesh = new Mesh();
			mesh.name = "MyMesh";
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			meshFilter.mesh = mesh;
		}
	}
}

