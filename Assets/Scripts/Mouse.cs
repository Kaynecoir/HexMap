using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Object;
using Karsss.Debug;

public class Mouse : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	public TestingHex testingHex;
	private void Update()
	{
		transform.position = DebugUtilites.GetMouseWorldPosition();

	}
}
