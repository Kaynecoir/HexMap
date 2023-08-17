using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class SoldierMoveControll : MonoBehaviour
{
	public float size = 1.0f;
	public Transform zeroCoordinate;
	public float speed;

	protected float positionX, positionY;
	protected SoldierData soldierData;
	protected Vector3 targetPos;
	protected List<HexPathNode> hexPathNodes;
	protected HexPathNode hexPosition;
	protected int index;

	private void Start()
	{
		soldierData = GetComponent<SoldierData>();
	}
	public void SetPosition(int x, int y, float size = 1.0f)
	{
		soldierData.indexX = x;
		soldierData.indexY = y;
		this.size = size;

		PathfindingHex.Instance.SetWalking(soldierData.indexX, soldierData.indexY, !PathfindingHex.Instance.GetGrid().GetValue(soldierData.indexX, soldierData.indexY).Value.IsWalking);
		transform.position = PathfindingHex.Instance.GetGrid().GetPositionFromWorld(x, y);
	}
	public void SetPosition(HexPathNode hexPos, float size = 1.0f)
	{
		hexPosition = hexPos;
		soldierData.indexX = hexPosition.X;
		soldierData.indexY = hexPosition.Y;
		this.size = size;

		transform.position = PathfindingHex.Instance.GetGrid()[hexPosition.Y, hexPosition.X].worldPosition;
	}
	public bool GoToPosition(Vector3 pos)
	{
		PathfindingHex.Instance.GetGrid().GetXY(pos, out int x, out int y);
		return GoToPosition(x, y);
	}
	public bool GoToPosition(int x, int y)
	{
		//PathfindingHex.Instance.GetGrid().GetXY(transform.position, out indexX, out indexY);
		hexPathNodes = PathfindingHex.Instance.FindPath(soldierData.indexX, soldierData.indexY, x, y, soldierData.stepRadius);
		index = 0;
		if(hexPathNodes != null)
		{
			targetPos = hexPathNodes[index].HexParant.worldPosition;

			soldierData.SetArrayPos(null, soldierData.indexX, soldierData.indexY);
			soldierData.SetArrayPos(soldierData, x, y);
		}

		return hexPathNodes != null;
	}
	private void Update()
	{
		if (hexPathNodes != null)
		{
			transform.position += (targetPos - (transform.position)).normalized * speed * Time.deltaTime;

			if (targetPos != null && Mathf.Abs((transform.position - targetPos).x) < 0.1f && Mathf.Abs((transform.position - targetPos).y) < 0.1f)
			{
				PathfindingHex.Instance.SetWalking(soldierData.indexX, soldierData.indexY, !PathfindingHex.Instance.GetGrid().GetValue(soldierData.indexX, soldierData.indexY).Value.IsWalking);

				PathfindingHex.Instance.GetGrid().GetXY(transform.position, out soldierData.indexX, out soldierData.indexY);
				PathfindingHex.Instance.SetWalking(soldierData.indexX, soldierData.indexY, !PathfindingHex.Instance.GetGrid().GetValue(soldierData.indexX, soldierData.indexY).Value.IsWalking);
				PathfindingHex.Instance.mapVisual.UpdateFindMapVisual();

				if (index < hexPathNodes.Count)
				{
					targetPos = hexPathNodes[index].HexParant.worldPosition;
					++index;
				}
				else if(index == hexPathNodes.Count)
				{
					hexPathNodes = null;
				}
			}
		}
	}
}
