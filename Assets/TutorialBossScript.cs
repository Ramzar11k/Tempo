using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TutorialBossScript : MonoBehaviour {

    GameObject player;
    PlayerScript playerScript;
    public Stage1Script stage1Script;
    NavMeshAgent nav;
    Animator anim;
    EnemyConstructor tutorialBoss;
    Transform firePoint;

    public Slider healthBar;

    public Transform powerUpPoint;

    public bool powerUP;
    public bool poweringUP;
    float distanceToPowerUp;

    public BoxCollider hammer;
    public GameObject bullet;
    BossAmmoScript bossAmmoScript;

    GameObject currentBullet;

    float distance;
    float rangeAttackCD;

    int health;

    float aggroRange = 6f;
    float attackRate = 3f;
    float closeAttackRange = 2.5f;

    bool active;
    bool attacking;

    public bool phase1;
    public bool phase2;
    public bool phase3;

    bool non;

    List<Color> phases = new List<Color>();

    int heak;
    
	void Start ()
    {
        attackRate = 3f;
        for (var i = 0; i < transform.GetChild(1).childCount; i++)
            phases.Add(transform.GetChild(1).GetChild(i).GetComponent<Renderer>().material.color);
        firePoint = transform.GetChild(0).GetChild(2).GetChild(4).GetChild(0);
        player = GameObject.FindGameObjectWithTag("Player");
        bossAmmoScript = bullet.GetComponent<BossAmmoScript>();
        playerScript = player.GetComponent<PlayerScript>();
        tutorialBoss = new EnemyConstructor("TutorialBoss", 300, 3, 0, 10);
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        heak = tutorialBoss.enemyHealth;
    }
	
	void Update () {
        if (player != null)
            distance = Vector3.Distance(transform.position, player.transform.position);
        distanceToPowerUp = Vector3.Distance(transform.position, powerUpPoint.position);
        ChangeColor();
        UIStuff();
        if (powerUP == true)
        {
            tutorialBoss.enemyArmor = 2;
            nav.enabled = true;
            nav.SetDestination(powerUpPoint.position);
            if (distanceToPowerUp <= 0.1f)
            {
                anim.SetTrigger("powerUp");
                nav.enabled = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                powerUP = false;
                non = true;
            }
            return;
        }

        if (tutorialBoss.enemyHealth <= 225 && phase1 == false)
        {
            phase1 = true;
            attackRate = 2.5f;
            bossAmmoScript.chargeSpeed = 1f;
            tutorialBoss.enemyDamage = 4;
            powerUP = true;
        }
        if (tutorialBoss.enemyHealth <= 150 && phase2 == false)
        {
            powerUP = true;
            attackRate = 2.2f;
            bossAmmoScript.chargeSpeed = 2.5f;
            tutorialBoss.enemyDamage = 5;
            phase2 = true;
        }
        if (tutorialBoss.enemyHealth <= 75 && phase3 == false)
        {
            powerUP = true;
            attackRate = 1.7f;
            bossAmmoScript.chargeSpeed = 6f;
            tutorialBoss.enemyDamage = 6;
            phase3 = true;
        }

        if (distance <= aggroRange && active == false)
        {
            active = true;
            nav.enabled = true;
            anim.SetBool("active", true);
        }
        if (active == false)
            return;

        if (non == false)
        {
            StartAttack();
            FollowPlayer();
        }
        FixRotation();

        if (tutorialBoss.enemyHealth <= 0 || playerScript.health <= 0)
        {
            healthBar.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        
    }

    void UIStuff()
    {
        if (active == true)
            healthBar.gameObject.SetActive(true);
        healthBar.value = ((float)tutorialBoss.enemyHealth / heak) * 100;
    }

    IEnumerator SlowDebuff()
    {
        var temp = playerScript.speed;
        playerScript.speed /= 2;
        yield return new WaitForSeconds(3f);
        playerScript.speed = temp;
        yield return null;
    }

    void StartAttack()
    {
        if (attacking == false)
        {
            if (distance > closeAttackRange && attackRate <= 0 && phase1 == true)
            {
                if (phase2 == false)
                {
                    nav.enabled = false;
                    anim.SetTrigger("shoot");
                    attacking = true;
                }
                else
                {
                    var x = Random.Range(0, 4);
                    if (x < 3)
                    {
                        nav.enabled = false;
                        anim.SetTrigger("shoot");
                    }
                    if (x == 3)
                    {
                        nav.enabled = false;
                        anim.SetTrigger("stomp");
                    }
                    attacking = true;
                }
            }
            else if (distance <= closeAttackRange && attackRate <= 0)
            {
                var x = Random.Range(0, 2);
                if (x == 0)
                {
                    nav.enabled = false;
                    anim.SetTrigger("attack1");
                }
                else if (x == 1)
                {
                    nav.enabled = false;
                    anim.SetTrigger("attack2");
                }
                attacking = true;
            }
        }

        if (attackRate > 0 && distance > closeAttackRange)
        {
            attackRate -= Time.deltaTime;
        }
        else if (attackRate > 0 && distance <= closeAttackRange)
            attackRate -= (Time.deltaTime * 2);
    }

    void ResetCD()
    {
        hammer.enabled = false;
        nav.enabled = true;
        attackRate = 3f;
        attacking = false;
    }

    void ActivateHammer()
    {
        hammer.enabled = true;
    }

    void ActivateShot()
    {
        currentBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        currentBullet.GetComponent<BossAmmoScript>().firePoint = firePoint;
    }

    void FireShot()
    {
        currentBullet.GetComponent<BossAmmoScript>().shot = true;
        currentBullet.GetComponent<BossAmmoScript>().damage = tutorialBoss.enemyDamage / 2;
    }

    void FollowPlayer()
    {
        transform.LookAt(player.transform);
        if (nav.enabled == true)
            nav.SetDestination(player.transform.position);
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

    IEnumerator PowerUP()
    {
        poweringUP = true;
        StartCoroutine(stage1Script.DrainBar(0.5f));
        yield return new WaitForSeconds(3f);
        tutorialBoss.enemyArmor = 0;
        nav.speed += 1;
        anim.speed += 0.65f;
        poweringUP = false;
        non = false;
        anim.SetTrigger("reengage");
        ResetCD();
        yield return null;
    }

    void ChangeColor()
    {
        if (poweringUP == true)
        {
            if (stage1Script.stageProgression == 8)
            {
                for (var i = 0; i < transform.GetChild(0).childCount; i++)
                {
                    for (var j = 0; j < transform.GetChild(0).GetChild(i).childCount; j++)
                    {
                        if (transform.GetChild(0).GetChild(i).GetChild(j).childCount > 0)
                        {
                            for (var k = 0; k < transform.GetChild(0).GetChild(i).GetChild(j).childCount; k++)
                                if (transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>() != null)
                                    transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>().material.color = Color.Lerp(transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>().material.color, phases[1], 0.02f);
                        }
                    }
                }
            }
            if (stage1Script.stageProgression == 9)
            {
                for (var i = 0; i < transform.GetChild(0).childCount; i++)
                {
                    for (var j = 0; j < transform.GetChild(0).GetChild(i).childCount; j++)
                    {
                        if (transform.GetChild(0).GetChild(i).GetChild(j).childCount > 0)
                        {
                            for (var k = 0; k < transform.GetChild(0).GetChild(i).GetChild(j).childCount; k++)
                                if (transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>() != null)
                                    transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>().material.color = Color.Lerp(transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>().material.color, phases[2], 0.01f);
                        }
                    }
                }
            }
            if (stage1Script.stageProgression == 10)
            {
                for (var i = 0; i < transform.GetChild(0).childCount; i++)
                {
                    for (var j = 0; j < transform.GetChild(0).GetChild(i).childCount; j++)
                    {
                        if (transform.GetChild(0).GetChild(i).GetChild(j).childCount > 0)
                        {
                            for (var k = 0; k < transform.GetChild(0).GetChild(i).GetChild(j).childCount; k++)
                                if (transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>() != null)
                                    transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>().material.color = Color.Lerp(transform.GetChild(0).GetChild(i).GetChild(j).GetChild(k).GetComponent<Renderer>().material.color, phases[3], 0.01f);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerScript>().health -= tutorialBoss.enemyDamage;
        if (other.CompareTag("Weapon"))
        {
            if (active == false)
            {
                active = true;
                nav.enabled = true;
                anim.SetBool("active", true);
            }
            if (playerScript.damage - tutorialBoss.enemyArmor >= 0)
                tutorialBoss.enemyHealth -= (playerScript.damage - tutorialBoss.enemyArmor);
        }
    }
}