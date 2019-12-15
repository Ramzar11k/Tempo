using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyConstructor
{
    public string enemyName;
    public int enemyHealth;
    public int enemyDamage;
    public int enemyArmor;
    public int enemyXP;

    public EnemyConstructor(string name, int health, int damage, int armor, int xp)
    {
        this.enemyName = name;
        this.enemyHealth = health;
        this.enemyDamage = damage;
        this.enemyArmor = armor;
        this.enemyXP = xp;
    }
}
