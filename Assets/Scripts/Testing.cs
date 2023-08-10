using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class Testing : MonoBehaviour
{
	//Grid grid;
	Grid<int> grid;
	
	PathfindingSquare pathfinding;
	FindMapVisual findMap;
	Material material;
	MeshFilter meshFilter;
	public int height, width;
	public float size;
	public int force;

	private void Start()
	{
		//grid = new Grid<int>(height, width, size, transform.position, () => 0, false);
		meshFilter = GetComponent<MeshFilter>();
		//HeatMapVisual heatMapVisual = new HeatMapVisual(grid, meshFilter, transform);
		
		pathfinding = new PathfindingSquare(height, width, size, transform.position);
		findMap = new FindMapVisual(pathfinding, meshFilter, transform);
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			findMap.ClearMap();
			Vector3 pos = DebugUtilites.GetMouseWorldPosition();
			pathfinding.GetGrid().GetXY(pos, out int x, out int y);
			List<PathNode> pathNodes = pathfinding.FindPath(x, y);

			pathfinding.SetStartNode(pathNodes[^1]);
			//grid.SetValue(pos, grid.GetValue(pos) + force );
		}
		if (Input.GetMouseButtonDown(1))
		{
			Vector3 pos = DebugUtilites.GetMouseWorldPosition();
			Debug.Log("Wall");
			pathfinding.GetGrid().GetValue(pos).IsWalking = !pathfinding.GetGrid().GetValue(pos).IsWalking;
			findMap.UpdateFindMapVisual();
		}
	}

	private class HeatMapVisual
	{
		private Grid<int> grid;
		MeshFilter meshFilter;
		Transform transform;

		Vector3[] vertices;
		Vector3[] normals;
		Vector2[] uv;
		int[] triangles;
		public HeatMapVisual(Grid<int> grid, MeshFilter meshFilter, Transform transform)
		{
			this.grid = grid;
			this.grid.ChangeValue += UpdateHeatMapVisual;
			this.meshFilter = meshFilter;
			this.transform = transform;

			KarsMesh.CreateEmptyMeshArray(grid.Height * grid.Width, out vertices, out normals, out uv, out triangles);

			UpdateHeatMapVisual();
		}

		public void UpdateHeatMapVisual()
		{
			Vector3 baseSize = new Vector3(1, 1) * grid.Size;
			int maxGridValue = 512;

			for (int y = 0; y < grid.Height; y++)
			{
				for (int x = 0; x < grid.Width; x++)
				{
					int index = y * grid.Width + x;
					int gridValue = grid.GetValue(x, y);
					float gridValueNormalized;
					Vector2 gridCellUV = new Vector2();

					if(gridValue > maxGridValue-8)
					{
						grid.SetValue(x, y, maxGridValue-8);
						gridValue = maxGridValue - 8;
					}
					gridValueNormalized = Mathf.Clamp01((float)(gridValue) / maxGridValue);
					gridCellUV = new Vector2(gridValueNormalized, 0.0f);
					KarsMesh.AddToMeshArray(vertices, normals, uv, triangles, index, -transform.position + grid.GetWorldPosition(x, y) + baseSize * 0.5f, 0f, baseSize, Vector3.zero, gridCellUV);
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

