using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Object;
using Kars.Debug;

public class TestingHex : MonoBehaviour
{
	HexGrid<int> hexGrid;
	public int height, width;
	public float radius;
	public bool isVertical;
	FollowCursorHexMap<int> followCursor;
	private void Start()
	{
		Vector3 pos = new Vector3(radius * (isVertical ? Mathf.Sin(Mathf.PI / 3): 1), radius * (!isVertical ? Mathf.Sin(Mathf.PI / 3) : 1));
		hexGrid = new HexGrid<int>(height, width, radius, transform.position, (int val) => 
		{
			Hexagon<int> h = new Hexagon<int>(radius);
			h.SetValue(val);
			return h; 
		}, pos, isVertical);
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = hexGrid.CreateMeshArray();
		followCursor = new FollowCursorHexMap<int>(hexGrid, meshFilter);
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{		
			Debug.Log("posMouse: " + DebugUtilites.GetMouseWorldPosition(transform.position));
			Debug.Log("pos: " + hexGrid.GetXY(DebugUtilites.GetMouseWorldPosition(transform.position)));
			
			//Debug.Log("hex: " + hex.inHexArea(DebugUtilites.GetMouseWorldPosition(transform.position)));

			Hexagon<int> h = hexGrid.GetValue(DebugUtilites.GetMouseWorldPosition(transform.position));
			//hexGrid.ClearHexMeshArray();
			followCursor.UpdateNodeOfMapVisual(h, true);
		}

	}

	class FollowCursorHexMap<HexObject>
	{
		HexGrid<HexObject> hexGrid;
		public MeshFilter meshFilter;
		public Transform transform;
		Vector3[] vertices;
		Vector3[] normals;
		Vector2[] uv;
		int[] triangles;

		public FollowCursorHexMap(HexGrid<HexObject> hexGrid, MeshFilter meshFilter)
		{
			this.hexGrid = hexGrid;
			vertices = hexGrid.gridMesh.vertices;
			normals = hexGrid.gridMesh.normals;
			uv = hexGrid.gridMesh.uv;
			triangles = hexGrid.gridMesh.triangles;
			this.meshFilter = meshFilter;
		}
		public void UpdateNodeOfMapVisual(Hexagon<HexObject> hex, bool isWhite)
		{
			Vector3 baseSize = new Vector3(1, 1) * hexGrid.Size;

			int x = hex.positionX, y = hex.positionY;
			int index = y * hexGrid.Width + x;
			Vector2 uvStep = new Vector2(0, 0);

			hexGrid.AddHexMeshToArray(hex, vertices, normals, uv, triangles, index, isWhite ? new Vector2(0.99f, 0) : new Vector2(0, 0));
			foreach(Hexagon<HexObject> h in hex.neigbourHex)
			{
				hexGrid.AddHexMeshToArray(h, vertices, normals, uv, triangles, h.positionY * hexGrid.Width + h.positionX, new Vector2(0.99f, 0.0f));
			}

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
