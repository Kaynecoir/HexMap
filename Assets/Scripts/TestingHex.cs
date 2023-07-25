using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Object;

public class TestingHex : MonoBehaviour
{
	HexGrid hexGrid;
	public bool isVertical;
	private void Start()
	{
		hexGrid = new HexGrid(3, 4, 10f, Vector3.zero, (HexGrid grid, int height, int width) => 
		{
			Hex h = new Hex(grid.Size / 2);
			return h; 
		});
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = hexGrid.CreateMeshArray();
	}

	private void OnDrawGizmosSelected()
	{

	}
}
