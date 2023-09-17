using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class TestingHex : MonoBehaviour
{
	public SoldierData[,] soldierArray;
	public FindHexMapVisual findMap;
	public int height, width;
	public float radius;
	public bool isVertical;
	public GameObject soldierObject;
	public List<SoldierData> soldList;
	public SoldierData currentSoldier;
	private void Start()
	{
		Vector3 pos = new Vector3(radius * (isVertical ? Mathf.Sin(Mathf.PI / 3) : 1), radius * (!isVertical ? Mathf.Sin(Mathf.PI / 3) : 1));
		PathfindingHex.Instance.SetGrid(height, width, radius, transform.position, isVertical);
		soldierArray = new SoldierData[height, width];

		MeshFilter meshFilter = GetComponent<MeshFilter>();

		findMap = new FindHexMapVisual(PathfindingHex.Instance, meshFilter, transform);
		PathfindingHex.Instance.mapVisual = findMap;

		foreach(SoldierData s in soldList)
		{

			s.moveControll.SetPosition(s.indexX, s.indexY);
			soldierArray[s.indexY, s.indexX] = s;
		}
		ChooseSoldier(soldList[0]);
		findMap.UpdateFindMapVisual();
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
			int x, y, seq;
			PathfindingHex.Instance.GetGrid().GetXY(pos, out x, out y, out seq);

			if (currentSoldier != null && soldierArray[y, x] != null && (currentSoldier.indexY != y || currentSoldier.indexX != x))
			{
				SoldierData targetSoldier = soldierArray[y, x];
				if (PathfindingHex.Instance.GetGrid().isVertical)
				{
					int tx = 0, ty = 0;
					if (seq == 0) { tx = x - 1 + y % 2; ty = y + 1; }
					else if (seq == 1) { tx = x - 1; ty = y; }
					else if (seq == 2) { tx = x - 1 + y % 2; ty = y - 1; }
					else if (seq == 3) { tx = x + y % 2; ty = y - 1; }
					else if (seq == 4) { tx = x + 1; ty = y; }
					else if (seq == 5) { tx = x + y % 2; ty = y + 1; }
					if (currentSoldier.moveControll.GoToPosition(tx, ty) || (currentSoldier.indexY == ty && currentSoldier.indexX == tx)) 
					{
						if (targetSoldier != null) currentSoldier.Attack(targetSoldier);
						NextSoldier(); 
					}
				}
				else
				{

				}
			}
			if (currentSoldier != null)
			{

				if (PathfindingHex.Instance.GetGrid().GetValue(pos).Value.IsWalking && currentSoldier.moveControll.GoToPosition(DebugUtilites.GetMouseWorldPosition()))
				{

					NextSoldier();
				}
			}
		}
	}

	public void ChooseSoldier(SoldierData soldier)
	{
		if (currentSoldier != null) currentSoldier.ui.spriteRenderer.color = Color.green;
		if (soldier != null) currentSoldier = soldier;

		if (currentSoldier != null) { currentSoldier.ui.spriteRenderer.color = Color.white; }
	}
	public void NextSoldier()
	{
		int i = soldList.IndexOf(currentSoldier);
		ChooseSoldier(soldList[(i + 1) < soldList.Count ? (i + 1) : 0]);
	}
}
