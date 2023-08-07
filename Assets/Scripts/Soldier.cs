using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Object;

public class Soldier : MonoBehaviour
{
	public int indexX, indexY;
	private float positionX, positionY;
	public float size = 1.0f;
	private GameObject square;
	private Vector3 targetPos;
	private float speed;

	public void SetPosition(int x, int y, float size = 1.0f)
	{
		indexX = x;
		indexY = y;
		this.size = size;

		square.transform.localScale = Vector3.one * size;
		transform.position = new Vector3(x, y) * size;
	}
	public void GoToPosition(Vector3Int pos)
	{
		GoToPosition(pos.x, pos.y);
	}
	public void GoToPosition(int x, int y)
	{
		List<HexPathNode> list = PathfindingHex.Instance.FindPath(indexX, indexY, x, y);

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
