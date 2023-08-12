using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class Soldier : MonoBehaviour
{
	public int indexX, indexY;
	public float size = 1.0f;
	public Transform zeroCoordinate;
	public float speed;

	protected float positionX, positionY;
	protected GameObject square;
	protected Vector3 targetPos;
	protected List<HexPathNode> hexPathNodes;
	protected HexPathNode hexPosition;
	protected int index;

	private void Start()
	{
		SetPosition(0, 0, 1);

	}
	public void SetPosition(int x, int y, float size = 1.0f)
	{
		indexX = x;
		indexY = y;
		this.size = size;

		transform.position = PathfindingHex.Instance.GetGrid().GetPositionFromWorld(x, y);
	}
	public void SetPosition(HexPathNode hexPos, float size = 1.0f)
	{
		hexPosition = hexPos;
		indexX = hexPosition.X;
		indexY = hexPosition.Y;
		this.size = size;

		transform.position = PathfindingHex.Instance.GetGrid()[hexPosition.Y, hexPosition.X].worldPosition;
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

			transform.position += (targetPos - (transform.position)).normalized * speed * Time.deltaTime;

			if (targetPos != null && Mathf.Abs((transform.position - targetPos).x) < 0.1f && Mathf.Abs((transform.position - targetPos).y) < 0.1f)
			{
				UnityEngine.Debug.Log("<-1" + indexX + " " + indexY + " = " + PathfindingHex.Instance.GetGrid().GetValue(indexX, indexY).Value.IsWalking);
				PathfindingHex.Instance.SetWalking(indexX, indexY, !PathfindingHex.Instance.GetGrid().GetValue(indexX, indexY).Value.IsWalking);
				UnityEngine.Debug.Log("<-2" + indexX + " " + indexY + " = " + PathfindingHex.Instance.GetGrid().GetValue(indexX, indexY).Value.IsWalking);

				PathfindingHex.Instance.GetGrid().GetXY(transform.position, out indexX, out indexY);
				UnityEngine.Debug.Log("->" + indexX + " " + indexY + " = " + PathfindingHex.Instance.GetGrid().GetValue(indexX, indexY).Value.IsWalking);
				PathfindingHex.Instance.SetWalking(indexX, indexY, !PathfindingHex.Instance.GetGrid().GetValue(indexX, indexY).Value.IsWalking);
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

		if (Input.GetMouseButtonDown(0))
		{
			GoToPosition(DebugUtilites.GetMouseWorldPosition());
		}
	}
}
