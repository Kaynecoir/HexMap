using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karsss.Object
{
	public class PathNode
	{
		public Grid<PathNode> Grid { get; }
		public int Y { get; set; }
		public int X { get; set; }
		public int GCost { get; set; }
		public int HCost { get; set; }
		public int FCost { get; set; }
		public bool IsWalking { get; set; }
		public PathNode CameFromNode { get; set; }
		public Hexagon<PathNode> HexParant { get; set; }

		public PathNode()
		{

		}
		public PathNode(Grid<PathNode> grid, int y, int x)
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
