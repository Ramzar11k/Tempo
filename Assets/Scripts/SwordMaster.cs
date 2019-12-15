using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordMaster : MonoBehaviour {

    float attackRate = 0.5f;

    PlayerScript playerScript;
    public Stage1Script stage1Script;

    public GameObject weapon;
    Transform attackPoint;

    float skillCD;
    float skillMaxCD = 15f;
    bool skillActive;

    GameObject masterStuff;
    Image cooldownIndicator;

    void Start ()
    {
        masterStuff = GameObject.FindGameObjectWithTag("UI").transform.GetChild(0).GetChild(4).gameObject;
        masterStuff.SetActive(true);
        cooldownIndicator = masterStuff.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        playerScript = GetComponent<PlayerScript>();
        playerScript.maxHealth = 15;
        playerScript.health = 15;
        playerScript.damage = 3;
        attackPoint = transform.GetChild(1).GetChild(0).transform;
	}
	
	void Update ()
    {
        Attack();
        if (Input.GetKeyDown(KeyCode.E) && skillCD <= 0)
            StartCoroutine(HealOverTime(5));
        UIStuff();
        if (skillCD > 0)
            skillCD -= Time.deltaTime;
    }

    void UIStuff()
    {
        cooldownIndicator.fillAmount = 1 - (skillCD / skillMaxCD);
    }

    IEnumerator HealOverTime(float duration)
    {
        skillCD = skillMaxCD;
        do
        {
            if (playerScript.health < playerScript.maxHealth)
                playerScript.health += 1;
            duration -= 1;
            yield return new WaitForSeconds(1);
        } while (duration > 0);
        yield return null;
    }

    void Attack()
    {
        var rotationVector = transform.rotation.eulerAngles;
        if (Input.GetKey(KeyCode.UpArrow) && playerScript.attacking == false)
        {
            rotationVector.y = 0;
            transform.rotation = Quaternion.Euler(rotationVector);
            StartCoroutine(BasicAttack());
        }
        if (Input.GetKey(KeyCode.DownArrow) && playerScript.attacking == false)
        {
            rotationVector.y = 180;
            transform.rotation = Quaternion.Euler(rotationVector);
            StartCoroutine(BasicAttack());
        }
        if (Input.GetKey(KeyCode.LeftArrow) && playerScript.attacking == false)
        {
            rotationVector.y = 270;
            transform.rotation = Quaternion.Euler(rotationVector);
            StartCoroutine(BasicAttack());
        }
        if (Input.GetKey(KeyCode.RightArrow) && playerScript.attacking == false)
        {
            rotationVector.y = 90;
            transform.rotation = Quaternion.Euler(rotationVector);
            StartCoroutine(BasicAttack());
        }
    }

    IEnumerator BasicAttack()
    {
        playerScript.attacking = true;
        Instantiate(weapon, attackPoint.position, attackPoint.rotation, attackPoint);
        yield return new WaitForSeconds(attackRate);
        playerScript.attacking = false;
        yield return null;
    }
}
