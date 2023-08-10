using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karsss.Object
{ 
	public class PathfindingSquare
	{
		private const int MOVE_STRAIGHT_COST = 10;
		private const int MOVE_DIAGONAL_COST = 14;

		private Grid<PathNode> grid;
		private PathNode startNode;
		private PathNode endNode;
		private List<PathNode> openList;
		private List<PathNode> closedList;

		public delegate void GridFunc(PathNode node);
		public event GridFunc AddToOpenList, AddToClosedList, FindWay, ChangeWalking;
		public PathfindingSquare(int height, int width, float size, Vector3 positionToWorld, bool isDebugging = false)
		{
			grid = new Grid<PathNode>(height, width, size, positionToWorld, (Grid<PathNode> grid, int y, int x) => new PathNode(grid, y, x));
			SetStartNode(0, 0);
			if (isDebugging) grid.SeeDebug();
		}

		public Grid<PathNode> GetGrid()
		{
			return grid;
		}
		public void SetWalking(Vector3 pos, bool value)
		{
			grid.GetXY(pos, out int x, out int y);
			PathNode node = grid[y, x];
			node.IsWalking = value;
			ChangeWalking?.Invoke(node);
		}
		public void SetStartNode(PathNode node)
		{
			startNode = node;
		}
		public void SetStartNode(int x, int y)
		{
			startNode = grid[y, x];
		}
		public List<PathNode> FindPath(int endX, int endY)
		{
			return FindPath(startNode.X, startNode.Y, endX, endY);
		}
		public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
		{
			startNode = grid.GetValue(startX, startY);
			endNode = grid.GetValue(endX, endY);
			openList = new List<PathNode> { startNode };
			AddToOpenList?.Invoke(startNode);
			closedList = new List<PathNode>();

			for(int y = 0; y < grid.Height; y++)
			{
				for(int x = 0; x < grid.Width; x++)
				{
					PathNode pathNode = grid.GetValue(x, y);
					pathNode.GCost = int.MaxValue;
					pathNode.CalculateFCost();
					pathNode.CameFromNode = null;
				}
			}
			startNode.GCost = 0;
			startNode.HCost = CalculateDistance(startNode, endNode);
			startNode.CalculateFCost();

			while(openList.Count > 0)
			{
				PathNode currentNode = GetLowerFCostNode(openList);
				if(currentNode == endNode)
				{
					// Final
					List<PathNode> way = CalculatePath(endNode);
					FindWay?.Invoke(endNode);
					return way;
				}

				openList.Remove(currentNode);
				closedList.Add(currentNode);
				AddToClosedList?.Invoke(currentNode);

				foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
				{
					if (closedList.Contains(neighbourNode)) continue;

					int tentativeGCost = currentNode.GCost + CalculateDistance(currentNode, neighbourNode);

					if(tentativeGCost < neighbourNode.GCost)
					{
						neighbourNode.CameFromNode = currentNode;
						neighbourNode.GCost = tentativeGCost;
						neighbourNode.HCost = CalculateDistance(neighbourNode, endNode);
						neighbourNode.CalculateFCost();

						if(!openList.Contains(neighbourNode))
						{
							openList.Add(neighbourNode);
							AddToOpenList?.Invoke(neighbourNode);
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
			int x = currentNode.X, y = currentNode.Y;
			if (x + 1 < grid.Width)
			{
				if (y + 1 < grid.Height && grid[y + 1, x + 1].IsWalking) neighbourList.Add(grid[y + 1, x + 1]); // right-up
				if (grid[y + 0, x + 1].IsWalking)						 neighbourList.Add(grid[y + 0, x + 1]); // right-middle
				if (y - 1 >= 0 && grid[y - 1, x + 1].IsWalking)			 neighbourList.Add(grid[y - 1, x + 1]); // right-dowm
			}

			if (y + 1 < grid.Height && grid[y + 1, x + 0].IsWalking)	 neighbourList.Add(grid[y + 1, x + 0]); // center-up
			if (y - 1 >= 0 && grid[y - 1, x + 0].IsWalking)				 neighbourList.Add(grid[y - 1, x + 0]); // center-down

			if(x - 1 >= 0)
			{
				if (y + 1 < grid.Height && grid[y + 1, x - 1].IsWalking) neighbourList.Add(grid[y + 1, x - 1]);	// left-up
				if (grid[y + 0, x - 1].IsWalking)						 neighbourList.Add(grid[y + 0, x - 1]);	// left-middle
				if (y - 1 >= 0 && grid[y - 1, x - 1].IsWalking)			 neighbourList.Add(grid[y - 1, x - 1]);	// left-down
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
			int distanceX = Mathf.Abs(a.X - b.X);
			int distanceY = Mathf.Abs(a.Y - b.Y);
			int remaining = Mathf.Abs(distanceY - distanceX);
			return remaining * MOVE_STRAIGHT_COST + Mathf.Min(distanceX, distanceY) * MOVE_DIAGONAL_COST;
		}
		private PathNode GetLowerFCostNode(List<PathNode> pathNodeList)
		{
			PathNode lowerFCostNode = pathNodeList[0];
			for(int i = 1; i < pathNodeList.Count; i++)
			{
				if(lowerFCostNode.FCost > pathNodeList[i].FCost)
				{
					lowerFCostNode = pathNodeList[i];
				}
			}
			return lowerFCostNode;
		}
	}

	class FindMapVisual
	{
		private Grid<PathNode> grid;
		MeshFilter meshFilter;
		Transform transform;

		Vector3[] vertices;
		Vector3[] normals;
		Vector2[] uv;
		int[] triangles;

		public FindMapVisual(PathfindingSquare pathfinding, MeshFilter meshFilter, Transform transform)
		{
			this.grid = pathfinding.GetGrid();
			this.grid.ChangeValue += UpdateFindMapVisual;
			this.meshFilter = meshFilter;
			this.transform = transform;
			pathfinding.AddToOpenList += (PathNode node) => { UpdateNodeOfMapVisual(node, 0.2f);/*UnityEngine.Debug.Log(node);*/ };
			pathfinding.AddToClosedList += (PathNode node) => { UpdateNodeOfMapVisual(node, 0.6f); };
			pathfinding.FindWay += (PathNode node) =>
			{
				PathNode current = node;
				while (current != null)
				{
					UpdateNodeOfMapVisual(current, 0.99f);
					current = current.CameFromNode;
				}
			};
			pathfinding.ChangeWalking += (PathNode node) => { UpdateNodeOfMapVisual(node, 0.1f); };

			KarsMesh.CreateEmptyMeshArray(grid.Height * grid.Width, out vertices, out normals, out uv, out triangles);

			UpdateFindMapVisual();
		}

		public void ClearMap()
		{
			UpdateFindMapVisual();
		}
		public void UpdateFindMapVisual()
		{
			Vector3 baseSize = new Vector3(1, 1) * grid.Size;

			for (int y = 0; y < grid.Height; y++)
			{
				for (int x = 0; x < grid.Width; x++)
				{
					int index = y * grid.Width + x;

					KarsMesh.AddToMeshArray(vertices, normals, uv, triangles, index, -transform.position + grid.GetWorldPosition(x, y) + baseSize * 0.5f, 0f, baseSize, Vector3.zero, grid.GetValue(x, y).IsWalking ? new Vector2(0.8f, 0.0f) : new Vector2(0.0f, 0.0f));
				}
			}
			Mesh mesh = new Mesh();
			mesh.name = "GridMesh";
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			meshFilter.mesh = mesh;
		}

		public void UpdateNodeOfMapVisual(PathNode node, float step)
		{
			Vector3 baseSize = new Vector3(1, 1) * grid.Size;

			int x = node.X, y = node.Y;
			int index = y * grid.Width + x;

			KarsMesh.AddToMeshArray(vertices, normals, uv, triangles, index, -transform.position + grid.GetWorldPosition(x, y) + baseSize * 0.5f, 0f, baseSize, Vector3.zero, new Vector2(step, 0.0f));

			Mesh mesh = new Mesh();
			mesh.name = "GridMesh";
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			meshFilter.mesh = mesh;
		}
	}
}
