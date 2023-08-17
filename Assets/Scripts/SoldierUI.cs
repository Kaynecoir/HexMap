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

    public Text textOfCount;
    public Text textOfHealth;
    public Text textOfName;

    void Start()
    {
        soldierData = GetComponent<SoldierData>();
        textOfCount.rectTransform.anchoredPosition = (Vector2)transform.position * 12f + new Vector2(0, -20);
    }

    // Update is called once per frame
    void Update()
    {
        textOfHealth.rectTransform.anchoredPosition = (Vector2)transform.position * 12f + new Vector2(0, 25);
        textOfName.rectTransform.anchoredPosition = (Vector2)transform.position * 12f + new Vector2(0, 45);
        textOfCount.rectTransform.anchoredPosition = (Vector2)transform.position * 12f + new Vector2(0, -25);

        textOfHealth.text = $"{soldierData.curHealth} / {soldierData.maxHealth}";
        textOfName.text = $"{soldierData.name}";
        textOfCount.text = soldierData.count.ToString();
    }
}
