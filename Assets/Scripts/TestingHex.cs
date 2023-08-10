using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class TestingHex : MonoBehaviour
{
	HexGrid<HexPathNode> hexGrid;
	PathfindingHex pathfinding;
	FindHexMapVisual findMap;

	public int height, width;
	public float radius;
	public bool isVertical;
	public GameObject soldierObject;
	private void Start()
	{
		Vector3 pos = new Vector3(radius * (isVertical ? Mathf.Sin(Mathf.PI / 3): 1), radius * (!isVertical ? Mathf.Sin(Mathf.PI / 3) : 1));
		//pathfinding = new PathfindingHex(height, width, radius, transform.position, isVertical);
		PathfindingHex.Instance.SetGrid(height, width, radius, transform.position, isVertical);
		//hexGrid = pathfinding.GetGrid();
		hexGrid = PathfindingHex.Instance.GetGrid();


		MeshFilter meshFilter = GetComponent<MeshFilter>();
		//meshFilter.mesh = hexGrid.CreateMeshArray();
		//findMap = new FindHexMapVisual(pathfinding, meshFilter, transform);
		//followCursor = new FollowCursorHexMap<HexPathNode>(hexGrid, meshFilter);
		findMap = new FindHexMapVisual(PathfindingHex.Instance, meshFilter, transform);
	}

	private void Update()
	{
		//if (Input.GetMouseButtonDown(0))
		//{
		//	findMap.ClearMap();
		//	Vector3 pos = DebugUtilites.GetMouseWorldPosition(transform.position);
		//	PathfindingHex.Instance.GetGrid().GetXY(pos, out int x, out int y);
		//	List<HexPathNode> pathNodes = PathfindingHex.Instance.FindPath(x, y);
		//	PathfindingHex.Instance.SetStartNode(pathNodes[^1]);
		//}
		if (Input.GetMouseButtonDown(1))
		{
			Vector3 pos = DebugUtilites.GetMouseWorldPosition(transform.position);

			//pathfinding.GetGrid().GetValue(pos).Value.IsWalking = !pathfinding.GetGrid().GetValue(pos).Value.IsWalking;
			PathfindingHex.Instance.SetWalking(pos, !PathfindingHex.Instance.GetGrid().GetValue(pos).Value.IsWalking);
			findMap.UpdateFindMapVisual();
		}
	}

	//class FollowCursorHexMap<HexObject> where HexObject : IHexObject
	//{
	//	HexGrid<HexObject> hexGrid;
	//	public MeshFilter meshFilter;
	//	public Transform transform;
	//	Vector3[] vertices;
	//	Vector3[] normals;
	//	Vector2[] uv;
	//	int[] triangles;

	//	public FollowCursorHexMap(HexGrid<HexObject> hexGrid, MeshFilter meshFilter)
	//	{
	//		this.hexGrid = hexGrid;
	//		vertices = hexGrid.gridMesh.vertices;
	//		normals = hexGrid.gridMesh.normals;
	//		uv = hexGrid.gridMesh.uv;
	//		triangles = hexGrid.gridMesh.triangles;
	//		this.meshFilter = meshFilter;
	//	}
	//	public void UpdateNodeOfMapVisual(Hexagon<HexObject> hex, bool isWhite)
	//	{
	//		int x = hex.positionX, y = hex.positionY;
	//		int index = y * hexGrid.Width + x;
	//		Vector2 uvStep = new Vector2(0, 0);

	//		hexGrid.AddHexMeshToArray(hex, vertices, normals, uv, triangles, index, isWhite ? new Vector2(0.99f, 0) : new Vector2(0, 0));
	//		foreach(Hexagon<HexObject> h in hex.neigbourHex)
	//		{
	//			hexGrid.AddHexMeshToArray(h, vertices, normals, uv, triangles, h.positionY * hexGrid.Width + h.positionX, new Vector2(0.99f, 0.0f));
	//		}

	//		Mesh mesh = hexGrid.gridMesh;
	//		mesh.name = "GridMesh";
	//		mesh.vertices = vertices;
	//		mesh.normals = normals;
	//		mesh.uv = uv;
	//		mesh.triangles = triangles;

	//		meshFilter.mesh = mesh;
	//	}
	//}
}
