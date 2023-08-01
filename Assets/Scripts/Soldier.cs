using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
	public int positionX, positionY;
	public float size = 1.0f;
	private GameObject square;

	private void Start()
	{
		
	}

	public void SetPosition(int x, int y, float size = 1.0f)
	{
		positionX = x;
		positionY = y;
		this.size = size;
		square.transform.localScale = Vector3.one * size;
		transform.position = new Vector3(x, y) * size;
	}
}
