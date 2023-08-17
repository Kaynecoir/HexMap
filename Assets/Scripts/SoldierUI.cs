using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Karsss.Debug;

public class SoldierUI : MonoBehaviour
{
    protected SoldierData soldierData;
    [SerializeField] protected Canvas canvas;

    public SpriteRenderer spriteRenderer;

    public Text textMesh;

    void Start()
    {
        soldierData = GetComponent<SoldierData>();
        textMesh.rectTransform.anchoredPosition = transform.position * 54;
    }

    // Update is called once per frame
    void Update()
    {
        //textMesh.rectTransform.anchoredPosition = (Vector2)transform.position * 12f + new Vector2(0, -20);
        //textMesh.text = soldierData.count.ToString();
    }
}
