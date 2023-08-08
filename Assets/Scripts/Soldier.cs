using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Object;
using Kars.Debug;

public class Soldier : MonoBehaviour
{
	public int indexX, indexY;
	private float positionX, positionY;
	public float size = 1.0f;
	private GameObject square;
	private Vector3 targetPos;
	private float speed;
	private List<HexPathNode> hexPathNodes;
	private int index;

	public void SetPosition(int x, int y, float size = 1.0f)
	{
		indexX = x;
		indexY = y;
		this.size = size;

		square.transform.localScale = Vector3.one * size;
		transform.position = new Vector3(x, y) * size;
	}
	public void GoToPosition(Vector3 pos)
	{
		PathfindingHex.Instance.GetGrid().GetXY(pos, out int x, out int y);
		GoToPosition(x, y);
	}
	public void GoToPosition(int x, int y)
	{
		hexPathNodes = PathfindingHex.Instance.FindPath(indexX, indexY, x, y);
		index = 0;
		targetPos = hexPathNodes[index].HexParant.worldPosition;
	}
	private void Update()
	{
		if (false)
		{
			transform.position += (transform.position - targetPos).normalized * speed * Time.deltaTime;
			if ((transform.position - targetPos).x < 0.01f && (transform.position - targetPos).y < 0.01f)
			{
				index++;
				targetPos = hexPathNodes[index].HexParant.worldPosition;
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			//GoToPosition(DebugUtilites.GetMouseWorldPosition(PathfindingHex.Instance.GetGrid));
		}
	}
	//public up
	////IEnumerator MoveToPath(int x, int y)
	////{
	////	foreach(HexPathNode node in PathfindingHex.Instance.FindPath(indexX, indexY, x, y))
	////	{

	////	}
	////}

	//IEnumerator Move()
	//{
	//	while(positionX - targetPos.x <= 0.01f && positionY - targetPos.y <= 0.01f)
	//	{
	//		transform.position += new Vector3(positionX - targetPos.x, positionY - targetPos.y).normalized * speed * Time.deltaTime;
	//		yield return null;
	//	}
	//}
}
