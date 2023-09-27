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
        // I have problem. Ok? I can't dream project and write code...
        // Yes. I still can't work. Today I'm sit 8 hour in army center
        // Yes. Today Too...
	    // Yes, I hate me...
        // Go to the Hell f***ing b****rd
        // He-elp me-e. I have so much problems... :(
        // ...
        // Yes, yes, yes, go to the hell
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
