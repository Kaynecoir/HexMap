using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class Mouse : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	public TestingHex testingHex;
	private void Update()
	{
		transform.position = DebugUtilites.GetMouseWorldPosition();

		if (Input.GetMouseButtonDown(0))
		{
			Vector3 pos = DebugUtilites.GetMouseWorldPosition();
			int x, y, seq;
			PathfindingHex.Instance.GetGrid().GetXY(pos, out x, out y, out seq);
			if (testingHex.soldierArray[y, x] != null && (testingHex.currentSoldier.indexY != y || testingHex.currentSoldier.indexX != x))
			{
				SoldierData currentSoldier = testingHex.currentSoldier;
				SoldierData targetSoldier = testingHex.soldierArray[y, x];
				if (PathfindingHex.Instance.GetGrid().isVertical)
				{
					int tx = 0, ty = 0;
					if (seq == 0) { tx = x - 1 + y % 2; ty = y + 1; }
					else if (seq == 1) { tx = x - 1; ty = y; }
					else if (seq == 2) { tx = x - 1 + y % 2; ty = y - 1; }
					else if (seq == 3) { tx = x + y % 2; ty = y - 1; }
					else if (seq == 4) { tx = x + 1; ty = y; }
					else if (seq == 5) { tx = x + y % 2; ty = y + 1; }
					//if (currentSoldier.moveControll.GoToPosition(tx, ty) || (testingHex.currentSoldier.indexY == ty && testingHex.currentSoldier.indexX == tx)) { testingHex.NextSoldier(); }
				}
				else
				{

				}
				//if(targetSoldier != null) currentSoldier.Attack(targetSoldier);
			}
		}
	}
}
