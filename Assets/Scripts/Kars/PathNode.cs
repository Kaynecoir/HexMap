using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kars.Object
{
	public class PathNode : IPathNode
	{
		public Grid<IPathNode> Grid { get; }
		public int Y { get; set; }
		public int X { get; set; }
		public int GCost { get; set; }
		public int HCost { get; set; }
		public int FCost { get; set; }
		public bool IsWalking { get; set; }
		IPathNode IPathNode.CameFromNode { get; set; }
		public Hexagon<IHexObject> HexParant { get; set; }

		public PathNode()
		{

		}
		public PathNode(Grid<IPathNode> grid, int y, int x)
		{
			this.Grid = grid;
			this.Y = y;
			this.X = x;
			IsWalking = true;
		}

		public void CalculateFCost()
		{
			FCost = HCost + GCost;
		}
		public override string ToString()
		{
			return X + ", " + Y + "\nh = " + HCost + " g = " + GCost + " F = " + FCost;
		}
	}
}
