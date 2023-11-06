using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierData : MonoBehaviour
{
    public TestingHex testingHex;

    public int maxHealth;
    public int curHealth;
    public int count = 1;
    public int attackPower;
    public int stepRadius;
    public int indexX = 0, indexY = 0;

    public delegate void SoldierInteraction(SoldierData targetSoldier);
    public event SoldierInteraction AttackSoldier;


    public SoldierMoveControll moveControll;
    public SoldierUI ui;
    void Start()
    {
        moveControll = GetComponent<SoldierMoveControll>();
        ui = GetComponent<SoldierUI>();
        AttackSoldier += (SoldierData targetSoldier) =>
        {
            targetSoldier.SetHealth(-attackPower * count);
        };
        AttackSoldier += (SoldierData targetSoldier) =>
        {
            Debug.Log($"{this.name} Attack {targetSoldier.name} on {attackPower * count}\nNow Army[{targetSoldier.count}] HP[{targetSoldier.curHealth} / {targetSoldier.maxHealth}]");
        };
        // 21.09.2023
        // 22.09.2023
        // 23.09.2023
        // 24.09.2023
        // 25.09.2023
        // 26.09.2023
        // 27.09.2023
        // 28.09.2023
        // 29.09.2023
        // 30.09.2023
        // 01.10.2023
        // 02.10.2023
        // 03.10.2023
        // 04.10.2023
        // 05.10.2023
        // 06.10.2023
        // 07.10.2023
        // 08.10.2023
        // 09.10.2023
        // 10.10.2023
        // 11.10.2023
        // 12.10.2023
        // 13.10.2023
        // 14.10.2023
        // 15.10.2023
	    // 16.10.2023
        // 17.10.2023
        // 18.10.2023
        // 19.10.2023
        // 20.10.2023
        // 21.10.2023
        // 22.10.2023
        // 23.10.2023
        // 24.10.2023
        // 25.10.2023
        // 26.10.2023
        // 27.10.2023
        // 28.10.2023
        // 29.10.2023
        // 30.10.2023
    	// 31.10.2023
        // 01.11.2023
        // 02.11.2023
        // 03.11.2023
	// 04.11.2023
        // 05.11.2023
	// 06.11.2023
    }

    void Update()
    {
        
    }

    public void SetCount(int c)
	{
        count += c;
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
            SetCount(-1);
        }
    }
    public void SetArrayPos(SoldierData soldier, int x, int y)
    {
        testingHex.soldierArray[y, x] = soldier;
    }
    public void Attack(SoldierData targetSoldier)
    {
        AttackSoldier?.Invoke(targetSoldier);
    }
}
