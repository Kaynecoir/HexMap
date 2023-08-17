using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierData : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public int count = 1;

    public TestingHex testingHex;

    public SoldierMoveControll moveControll;
    public SoldierUI ui;
    void Start()
    {
        Debug.Log("1 " + name);
        moveControll = GetComponent<SoldierMoveControll>();
        ui = GetComponent<SoldierUI>();
    }

    void Update()
    {
        
    }

    public void SetCount(int c)
	{
        count -= c;
        if (count <= 0)
        {
            testingHex.soldList.Remove(this);
            Destroy(gameObject);
        }
	}
    public void SetHealth(int health)
    {
        curHealth += health;
        if (curHealth > maxHealth) curHealth = maxHealth;
        while (curHealth <= 0)
        {
            curHealth += maxHealth;
        }
    }

}
