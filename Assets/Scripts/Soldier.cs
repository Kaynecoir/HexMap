using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class Soldier : MonoBehaviour
{
	public int indexX, indexY;
	private float positionX, positionY;
	public float size = 1.0f;
	private GameObject square;
	private Vector3 targetPos;
	public Transform zeroCoordinate;
	public float speed;
	private List<HexPathNode> hexPathNodes;
	private int index;

	private void Start()
	{
		SetPosition(0, 0, 1);

	}
	public void SetPosition(int x, int y, float size = 1.0f)
	{
		indexX = x;
		indexY = y;
		this.size = size;

		transform.position = PathfindingHex.Instance.GetGrid().GetPositionFromCenter(x, y) + zeroCoordinate.position;
	}
	public void GoToPosition(Vector3 pos)
	{
		PathfindingHex.Instance.GetGrid().GetXY(pos, out int x, out int y);
		GoToPosition(x, y);
	}
	public void GoToPosition(int x, int y)
	{
		//PathfindingHex.Instance.GetGrid().GetXY(transform.position, out indexX, out indexY);
		hexPathNodes = PathfindingHex.Instance.FindPath(indexX, indexY, x, y);
		index = 0;
		targetPos = hexPathNodes[index].HexParant.worldPosition;
	}
	private void Update()
	{
		if (hexPathNodes != null)
		{

			transform.position += (targetPos - (transform.position - zeroCoordinate.position)).normalized * speed * Time.deltaTime;

			if (targetPos != null && Mathf.Abs((transform.position - zeroCoordinate.position - targetPos).x) < 0.1f && Mathf.Abs((transform.position - zeroCoordinate.position - targetPos).y) < 0.1f)
			{
				PathfindingHex.Instance.GetGrid().GetXY(transform.position - zeroCoordinate.position, out indexX, out indexY);
				if (index < hexPathNodes.Count)
				{
					targetPos = hexPathNodes[index].HexParant.worldPosition;
					++index;
				}
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			GoToPosition(DebugUtilites.GetMouseWorldPosition(zeroCoordinate.position));
		}
	}
}
