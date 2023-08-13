using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class TestingHex : MonoBehaviour
{
	HexGrid<HexPathNode> hexGrid;
	PathfindingHex pathfinding;
	public FindHexMapVisual findMap;

	public int height, width;
	public float radius;
	public bool isVertical;
	public GameObject soldierObject;
	public List<Soldier> soldList;
	public Soldier? currentSoldier;
	private void Start()
	{
		Vector3 pos = new Vector3(radius * (isVertical ? Mathf.Sin(Mathf.PI / 3) : 1), radius * (!isVertical ? Mathf.Sin(Mathf.PI / 3) : 1));
		PathfindingHex.Instance.SetGrid(height, width, radius, transform.position, isVertical);
		hexGrid = PathfindingHex.Instance.GetGrid();

		foreach(Soldier s in soldList)
		{
			s.SetPosition(s.indexX, s.indexY);

		}

		MeshFilter meshFilter = GetComponent<MeshFilter>();

		findMap = new FindHexMapVisual(PathfindingHex.Instance, meshFilter, transform);
		PathfindingHex.Instance.mapVisual = findMap;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Vector3 pos = DebugUtilites.GetMouseWorldPosition();

			PathfindingHex.Instance.SetWalking(pos, !PathfindingHex.Instance.GetGrid().GetValue(pos).Value.IsWalking);

			findMap.UpdateFindMapVisual();
		}
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 pos = DebugUtilites.GetMouseWorldPosition();

			if (currentSoldier != null)
			{
				currentSoldier.GoToPosition(DebugUtilites.GetMouseWorldPosition());
			}
			PathfindingHex.Instance.GetGrid().GetXY(pos, out int x, out int y);
			foreach (Soldier s in soldList)
			{
				if(s.indexX == x && s.indexY == y)
				{
					if (currentSoldier != null) currentSoldier.spriteRenderer.color = Color.green;
					currentSoldier = s;
					currentSoldier.spriteRenderer.color = Color.white;
				}
			}	
		}
	}
}
