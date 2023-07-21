using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldText : MonoBehaviour
{
	public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int soringOrder)
	{
		GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
		Transform transform = gameObject.transform;
		transform.SetParent(parent, false);
		transform.localPosition = localPosition;
		TextMesh textObj = gameObject.GetComponent<TextMesh>();
		textObj.anchor = textAnchor;
		textObj.alignment = textAlignment;
		textObj.text = text;
		textObj.fontSize = fontSize;
		textObj.color = color;
		textObj.GetComponent<MeshRenderer>().sortingOrder = soringOrder;

		return textObj;
	}

	public static Vector3 GetMouseWorldPosition()
	{
		return GetMouseWorldPosition(Input.mousePosition, Camera.main);
	}
	public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
	{
		Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
		return worldPosition;
	}
}
