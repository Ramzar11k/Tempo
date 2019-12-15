using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulScript : MonoBehaviour {

    EnemyConstructor ghoul;
    EnemyScript enemyScript;
    NavMeshAgent nav;
    Transform player;
    Transform dashSpot;
    Vector3 dashPosition;
    Rigidbody rb;
    Animator anim;
    
    public float distance;
    public bool attacking;
    bool active;

    int maxHP;

    float aggroRange = 3f;
    float attackRange = 1.5f;

    void Awake()
    {
        ghoul = new EnemyConstructor("Ghoul", 10, 1, 0, 1);
        maxHP = ghoul.enemyHealth;
    }
    void Start ()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        dashSpot = transform.GetChild(1).GetChild(0);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyScript = GetComponent<EnemyScript>();
        enemyScript.health = ghoul.enemyHealth;
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
        if (distance <= attackRange && nav.enabled == true)
        {
            nav.enabled = false;
            rb.isKinematic = false;
            anim.SetTrigger("attacking");
        }
        FollowPlayer();
        FixRotation();
        ActivateFromDamage();

        if (attacking == true)
            transform.position = Vector3.Lerp(transform.position, dashPosition, 0.08f);
    }

    void FollowPlayer()
    {
        if (nav.enabled == true)
        {
            transform.LookAt(player);
            nav.SetDestination(player.position);
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

    void Dash()
    {
        enemyScript.damage = ghoul.enemyDamage;
        dashPosition = dashSpot.position;
        attacking = true;
    }

    void StopDash()
    {
        enemyScript.damage = 0;
        transform.LookAt(player);
        nav.enabled = true;
        rb.isKinematic = true;
        attacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerScript>().health -= enemyScript.damage;
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