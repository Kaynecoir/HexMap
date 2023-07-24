using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kars.Object
{
	class PathNode
	{
		private Grid<PathNode> grid;
		public int y, x;

		public int gCost, hCost, fCost;

		public bool isWalking;

		public PathNode CameFromNode;
		public PathNode()
		{

		}
		public PathNode(Grid<PathNode> grid, int y, int x)
		{
			this.grid = grid;
			this.y = y;
			this.x = x;
			isWalking = true;
		}

		public void CalculateFCost()
		{
			fCost = hCost + gCost;
		}
		public override string ToString()
		{
			return x + ", " + y + "\nh = " + hCost + " g = " + gCost + " F = " + fCost;
		}
	}
}
