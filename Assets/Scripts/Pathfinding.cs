using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Object;
class Pathfinding
{
	private const int MOVE_STRAIGHT_COST = 10;
	private const int MOVE_DIAGONAL_COST = 14;

	private Grid<PathNode> grid;
	private List<PathNode> openList;
	private List<PathNode> closedList;
	public Pathfinding(int height, int width, float size, Vector3 positionToWorld)
	{
		grid = new Grid<PathNode>(height, width, size, positionToWorld, (Grid<PathNode> grid, int y, int x) => new PathNode(grid, y, x));
		Debug.Log(grid);
		grid.SeeDebug();
	}

	public Grid<PathNode> GetGrid()
	{
		return grid;
	}
	public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
	{
		PathNode startNode = grid.GetValue(startX, startY);
		PathNode endNode = grid.GetValue(endX, endY);
		openList = new List<PathNode> { startNode };
		closedList = new List<PathNode>();

		for(int y = 0; y < grid.Height; y++)
		{
			for(int x = 0; x < grid.Width; x++)
			{
				PathNode pathNode = grid.GetValue(x, y);
				pathNode.gCost = int.MaxValue;
				pathNode.CalculateFCost();
				pathNode.CameFromNode = null;
			}
		}
		startNode.gCost = 0;
		startNode.hCost = CalculateDistance(startNode, endNode);
		startNode.CalculateFCost();

		while(openList.Count > 0)
		{
			PathNode currentNode = GetLowerFCostNode(openList);
			if(currentNode == endNode)
			{
				// Final
				return CalculatePath(endNode);
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
			{
				if (closedList.Contains(neighbourNode)) continue;

				int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);

				if(tentativeGCost < neighbourNode.gCost)
				{
					neighbourNode.CameFromNode = currentNode;
					neighbourNode.gCost = tentativeGCost;
					neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
					neighbourNode.CalculateFCost();

					if(!openList.Contains(neighbourNode))
					{
						openList.Add(neighbourNode);
					}
				}
			}
		}
		// way not found
		return null;
	}

	private List<PathNode> GetNeighbourList(PathNode currentNode)
	{
		List<PathNode> neighbourList = new List<PathNode>();
		int x = currentNode.x, y = currentNode.y;
		if (x + 1 < grid.Width)
		{
			if (y + 1 < grid.Height && grid[y + 1, x + 1].isWalking) neighbourList.Add(grid[y + 1, x + 1]); // right-up
			if (grid[y + 0, x + 1].isWalking)						 neighbourList.Add(grid[y + 0, x + 1]); // right-middle
			if (y - 1 >= 0 && grid[y - 1, x + 1].isWalking)			 neighbourList.Add(grid[y - 1, x + 1]); // right-dowm
		}

		if (y + 1 < grid.Height && grid[y + 1, x + 0].isWalking)	 neighbourList.Add(grid[y + 1, x + 0]); // center-up
		if (y - 1 >= 0 && grid[y - 1, x + 0].isWalking)				 neighbourList.Add(grid[y - 1, x + 0]); // center-down

		if(x - 1 >= 0)
		{
			if (y + 1 < grid.Height && grid[y + 1, x - 1].isWalking) neighbourList.Add(grid[y + 1, x - 1]);	// left-up
			if (grid[y + 0, x - 1].isWalking)						 neighbourList.Add(grid[y + 0, x - 1]);	// left-middle
			if (y - 1 >= 0 && grid[y - 1, x - 1].isWalking)			 neighbourList.Add(grid[y - 1, x - 1]);	// left-down
		}		

		return neighbourList;
	}
	private List<PathNode> CalculatePath(PathNode endNode)
	{
		List<PathNode> pathNodes = new List<PathNode> { endNode };
		PathNode nextPathStep = endNode.CameFromNode;
		while(nextPathStep != null)
		{
			pathNodes.Add(nextPathStep);
			nextPathStep = nextPathStep.CameFromNode;
		}
		pathNodes.Reverse();
		return pathNodes;
	}
	private int CalculateDistance(PathNode a, PathNode b)
	{
		int distanceX = Mathf.Abs(a.x - b.x);
		int distanceY = Mathf.Abs(a.y - b.y);
		int remaining = Mathf.Abs(distanceY - distanceX);
		return remaining * MOVE_STRAIGHT_COST + Mathf.Min(distanceX, distanceY) * MOVE_DIAGONAL_COST;
	}
	private PathNode GetLowerFCostNode(List<PathNode> pathNodeList)
	{
		PathNode lowerFCostNode = pathNodeList[0];
		for(int i = 1; i < pathNodeList.Count; i++)
		{
			if(lowerFCostNode.fCost > pathNodeList[i].fCost)
			{
				lowerFCostNode = pathNodeList[i];
			}
		}
		return lowerFCostNode;
	}
}
