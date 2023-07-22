using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	Grid grid;
	Material material;
	MeshFilter meshFilter;
	public int height, width;
	public float size;
	public int force;

	private void Start()
	{
		grid = new Grid(height, width, size, new Vector3(0, 0));
		meshFilter = GetComponent<MeshFilter>();
		HeatMapVisual heatMapVisual = new HeatMapVisual(grid, meshFilter);
		//GetComponent<MeshFilter>().mesh = WorldText.CreateTileMesh(grid.GetHeight(), grid.GetWidth(), grid.GetCellSize());
	}
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 pos = WorldText.GetMouseWorldPosition(this.transform);
			grid.SetValue(pos, grid.GetValue(pos) + force);
		}
		if (Input.GetMouseButtonDown(1))
		{
			Vector3 pos = WorldText.GetMouseWorldPosition(this.transform);
			Debug.Log(grid.GetValue(pos));
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

			UpdateHeatMapVisual();
		}

		public void UpdateHeatMapVisual()
		{
			Vector3[] vertices;
			Vector3[] normals;
			Vector2[] uv;
			int[] triangles;

			WorldText.CreateEmptyMeshArray(grid.GetHeight() * grid.GetWidth(), out vertices, out normals, out uv, out triangles);
			Vector3 baseSize = new Vector3(1, 1) * grid.GetCellSize();
			int maxGridValue = 1024;

			for (int y = 0; y < grid.GetHeight(); y++)
			{
				for (int x = 0; x < grid.GetWidth(); x++)
				{
					int index = y * grid.GetWidth() + x;
					int gridValue = grid.GetValue(x, y);
					float gridValueNormalized0, gridValueNormalized1;
					Vector2 gridCellUV = new Vector2();
					if (gridValue < 0.9f*256)
					{
						gridValueNormalized0 = Mathf.Clamp01((float)gridValue / 256);
						gridCellUV = new Vector2(0.0f, gridValueNormalized0);
					}
					else if(gridValue >= 0.9f*256 && gridValue < 0.9f*1024)
					{
						gridValueNormalized1 = Mathf.Clamp01((float)(gridValue - 0.9f * 256) / 768);
						gridCellUV = new Vector2(gridValueNormalized1, 0.9f);
					}
					else
					{
						gridCellUV = new Vector2(0.9f, 0.9f);
					}
					if(gridValue > maxGridValue-8)
					{
						grid.SetValue(x, y, maxGridValue-8);
						gridValue = maxGridValue - 8;
					}
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

