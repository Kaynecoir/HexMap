using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Object;
using Kars.Debug;

public class Testing : MonoBehaviour
{
	//Grid grid;
	Grid<int> grid;
	Pathfinding pathfinding;
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
		
		pathfinding = new Pathfinding(height, width, size, transform.position);
		findMap = new FindMapVisual(pathfinding.GetGrid(), meshFilter, transform);
	}
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 pos = DebugUtilites.GetMouseWorldPosition();
			pathfinding.GetGrid().GetXY(pos, out int x, out int y);
			List<PathNode> pathNodes = pathfinding.FindPath(0, 0, x, y);

			if(pathNodes != null)
			{
				for(int i = 0; i < pathNodes.Count - 1; i++)
				{
					Vector3 point1 = pathfinding.GetGrid().GetWorldPosition(pathNodes[i].x, pathNodes[i].y) + new Vector3(pathfinding.GetGrid().Size, pathfinding.GetGrid().Size) * 0.5f;
					Vector3 point2 = pathfinding.GetGrid().GetWorldPosition(pathNodes[i+1].x, pathNodes[i+1].y) + new Vector3(pathfinding.GetGrid().Size, pathfinding.GetGrid().Size) * 0.5f;
					Debug.DrawLine(point1, point2, Color.cyan, 5.0f);
				}
			}
			//grid.SetValue(pos, grid.GetValue(pos) + force );
		}
		if (Input.GetMouseButtonDown(1))
		{
			Vector3 pos = DebugUtilites.GetMouseWorldPosition();
			Debug.Log("Wall");
			pathfinding.GetGrid().GetValue(pos).isWalking = !pathfinding.GetGrid().GetValue(pos).isWalking;
			findMap.UpdateFindMapVisual();
		}
	}

	private class FindMapVisual
	{
		private Grid<PathNode> grid;
		MeshFilter meshFilter;
		Transform transform;

		Vector3[] vertices;
		Vector3[] normals;
		Vector2[] uv;
		int[] triangles;

		public FindMapVisual(Grid<PathNode> grid, MeshFilter meshFilter, Transform transform)
		{
			this.grid = grid;
			this.grid.ChangeValue += UpdateFindMapVisual;
			this.meshFilter = meshFilter;
			this.transform = transform;

			KarsMesh.CreateEmptyMeshArray(grid.Height * grid.Width, out vertices, out normals, out uv, out triangles);

			UpdateFindMapVisual();
		}

		public void UpdateFindMapVisual()
		{
			Vector3 baseSize = new Vector3(1, 1) * grid.Size;

			for (int y = 0; y < grid.Height; y++)
			{
				for (int x = 0; x < grid.Width; x++)
				{
					int index = y * grid.Width + x;
					
					KarsMesh.AddToMeshArray(vertices, normals, uv, triangles, index, -transform.position + grid.GetWorldPosition(x, y) + baseSize * 0.5f, 0f, baseSize, Vector3.zero, grid.GetValue(x, y).isWalking ? new Vector2(0.8f, 0.0f) : new Vector2(0.0f, 0.0f));
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

