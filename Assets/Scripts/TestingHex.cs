using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Object;
using Kars.Debug;

public class TestingHex : MonoBehaviour
{
	HexGrid hexGrid;
	Hex hex;
	public int height, width;
	public float size;
	public bool isVertical;
	FollowCursorHexMap followCursor;
	private void Start()
	{
		Vector3 pos = new Vector3(size * (isVertical ? Mathf.Sin(Mathf.PI / 3): 1), size * (!isVertical ? Mathf.Sin(Mathf.PI / 3) : 1));
		hexGrid = new HexGrid(height, width, size, transform.position, (HexGrid grid, int height, int width) => 
		{
			Hex h = new Hex(grid.Radius);
			return h; 
		}, pos, isVertical);
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = hexGrid.CreateMeshArray();
		followCursor = new FollowCursorHexMap(hexGrid, meshFilter);
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{		
			Debug.Log("posMouse: " + DebugUtilites.GetMouseWorldPosition(transform.position));
			Debug.Log("pos: " + hexGrid.GetXY(DebugUtilites.GetMouseWorldPosition(transform.position)));
			
			//Debug.Log("hex: " + hex.inHexArea(DebugUtilites.GetMouseWorldPosition(transform.position)));

		}
		Hex h = hexGrid.GetValue(DebugUtilites.GetMouseWorldPosition(transform.position));
		//hexGrid.ClearHexMeshArray();
		followCursor.UpdateNodeOfMapVisual(h, h.inHexArea(DebugUtilites.GetMouseWorldPosition(transform.position)));

	}

	class FollowCursorHexMap
	{
		HexGrid hexGrid;
		public MeshFilter meshFilter;
		public Transform transform;
		Vector3[] vertices;
		Vector3[] normals;
		Vector2[] uv;
		int[] triangles;

		public FollowCursorHexMap(HexGrid hexGrid, MeshFilter meshFilter)
		{
			this.hexGrid = hexGrid;
			vertices = hexGrid.gridMesh.vertices;
			normals = hexGrid.gridMesh.normals;
			uv = hexGrid.gridMesh.uv;
			triangles = hexGrid.gridMesh.triangles;
			this.meshFilter = meshFilter;
		}
		public void UpdateNodeOfMapVisual(Hex hex, bool isWhite)
		{
			Vector3 baseSize = new Vector3(1, 1) * hexGrid.Size;

			int x = hex.positionX, y = hex.positionY;
			int index = y * hexGrid.Width + x;
			Vector2 uvStep = new Vector2(0, 0);

			hexGrid.AddHexMeshToArray(hex, vertices, normals, uv, triangles, index, isWhite ? new Vector2(0.999f, 0) : new Vector2(0, 0));

			Mesh mesh = hexGrid.gridMesh;
			mesh.name = "GridMesh";
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			meshFilter.mesh = mesh;
		}
	}
}
