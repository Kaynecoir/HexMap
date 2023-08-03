using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kars.Object
{
	public interface IPathNode
	{
		public Grid<IPathNode> Grid { get; }
		public int Y { get; set; }
		public int X  { get; set; }

		public int GCost { get; set; }
		public int HCost { get; set; }
		public int FCost { get; set; }

		public bool IsWalking { get; set; }

		public IPathNode CameFromNode { get; set; }

		public Hexagon<IHexObject> HexParant { get; set; }

		public void CalculateFCost();
	}
}
