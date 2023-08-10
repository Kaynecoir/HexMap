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
		Vector3 pos = new Vector3(radius * (isVertical ? Mathf.Sin(Mathf.PI / 3) : 1), radius * (!isVertical ? Mathf.Sin(Mathf.PI / 3) : 1));
		PathfindingHex.Instance.SetGrid(height, width, radius, transform.position, isVertical);
		hexGrid = PathfindingHex.Instance.GetGrid();


		MeshFilter meshFilter = GetComponent<MeshFilter>();

		findMap = new FindHexMapVisual(PathfindingHex.Instance, meshFilter, transform);
	}

	private void Update()
	{

		if (Input.GetMouseButtonDown(1))
		{
			Vector3 pos = DebugUtilites.GetMouseWorldPosition();

			PathfindingHex.Instance.SetWalking(pos, !PathfindingHex.Instance.GetGrid().GetValue(pos).Value.IsWalking);

			findMap.UpdateFindMapVisual();
		}
	}
}
