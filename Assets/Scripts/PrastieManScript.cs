using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrastieManScript : MonoBehaviour {

    EnemyConstructor prastieMan;
    EnemyScript enemyScript;
    ProjectileScript projectileScript;
    NavMeshAgent nav;
    Transform player;
    Animator anim;

    Transform firePoint;
    public GameObject projectile;

    bool active;
    bool attacking;

    public float distance;
    float aggroRange = 4.5f;
    float attackRange = 6f;

    int maxHP;

    void Awake()
    {
        prastieMan = new EnemyConstructor("PrastieMan", 9, 2, 0, 5);
        maxHP = prastieMan.enemyHealth;
        projectileScript = projectile.GetComponent<ProjectileScript>();
        projectileScript.damage = prastieMan.enemyDamage;
        projectileScript.speed = 6f;
        projectileScript.duration = 1.5f;
        projectileScript.reflectable = false;
        projectileScript.destroyable = false;
    }

    void Start ()
    {
        firePoint = transform.GetChild(1).GetChild(0);
        enemyScript = GetComponent<EnemyScript>();
        enemyScript.health = prastieMan.enemyHealth;
        enemyScript.damage = prastieMan.enemyDamage;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
	}
	
	void Update ()
    {
        if (player != null)
            distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= aggroRange && active == false)
        {
            active = true;
            anim.SetBool("active", true);
            nav.enabled = true;
        }
        if (distance <= attackRange && nav.enabled == true && active == true)
        {
            nav.enabled = false;
            anim.SetTrigger("attacking");
            anim.SetBool("inAttack",true);
        }
        if (distance > attackRange && active == true)
        {
            nav.enabled = true;
            anim.SetBool("inAttack", false);
        }
        FollowPlayer();
        FixRotation();
        ActivateFromDamage();
    }

    void FollowPlayer()
    {
        firePoint.transform.LookAt(player);
        if (active == true)
        {
            transform.LookAt(player);
            if (nav.enabled == true)
            {
                nav.SetDestination(player.position);
            }
        }
    }
    void FixRotation()
    {
        float y = transform.localEulerAngles.y;
        float x = transform.eulerAngles.x;
        float z = transform.eulerAngles.z;
        if (x != 0 || z != 0)
        {
            Vector3 v3;
            v3.x = 0;
            v3.y = transform.localEulerAngles.y;
            v3.z = 0;
            transform.eulerAngles = v3;
        }
    }

    void Shoot()
    {
        Instantiate(projectile, firePoint.position, firePoint.rotation, firePoint);
    }

    void ActivateFromDamage()
    {
        if (enemyScript.health < maxHP && active == false)
        {
            active = true;
            anim.SetBool("active", true);
            nav.enabled = true;
        }
    }
}
