using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour {

    EnemyConstructor shooter;
    EnemyScript enemyScript;
    ProjectileScript projectileScript;
    public GameObject projectile;

    Transform firePoint;

    float fireRate = 1.5f;
    bool fireCD;

    private void Awake()
    {
        shooter = new EnemyConstructor("Shooter", 10, 2, 0, 5);
        projectileScript = projectile.GetComponent<ProjectileScript>();
        projectileScript.damage = shooter.enemyDamage;
        projectileScript.speed = 5f;
        projectileScript.duration = 3f;
        projectileScript.reflectable = true;
        projectileScript.destroyable = false;
    }

    void Start ()
    {
        firePoint = transform.GetChild(1).GetChild(0);
        enemyScript = GetComponent<EnemyScript>();
        enemyScript.health = shooter.enemyHealth;
        enemyScript.damage = shooter.enemyDamage;
    }
	
	void Update ()
    {
        if(fireCD == false)
            StartCoroutine(Fire());
	}

    IEnumerator Fire()
    {
        fireCD = true;
        yield return new WaitForSeconds(fireRate);
        Instantiate(projectile, firePoint.position, firePoint.rotation, firePoint);
        fireCD = false;
        yield return null;
    }
}
