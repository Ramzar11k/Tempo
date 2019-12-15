using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archer : MonoBehaviour
{

    float attackRate = 0.7f;
    public Stage1Script stage1Script;

    PlayerScript playerScript;

    public GameObject weapon;
    Transform attackPoint;

    int ammo = 5;
    int maxammo = 5;
    float reloadCD = 0;
    bool reloading;

    float skillCD;
    float skillMaxCD = 5f;

    GameObject ammoStuff;
    Rigidbody rb;
    Image cooldownIndicator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ammoStuff = GameObject.FindGameObjectWithTag("UI").transform.GetChild(0).GetChild(3).gameObject;
        ammoStuff.SetActive(true);
        cooldownIndicator = ammoStuff.transform.GetChild(4).GetChild(0).GetComponent<Image>();
        playerScript = GetComponent<PlayerScript>();
        playerScript.maxHealth = 10;
        playerScript.health = 10;
        playerScript.damage = 4;
        attackPoint = transform.GetChild(1).GetChild(0).transform;
    }

    void Update()
    {
        if (reloadCD > 0)
            reloadCD -= Time.deltaTime;
        Attack();
        UIStuff();
        if (Input.GetKeyDown(KeyCode.E) && skillCD <= 0)
            StartCoroutine(JumpBack(500, 1));
        if (skillCD > 0)
            skillCD -= (Time.deltaTime/2);
    }

    IEnumerator JumpBack (float force, float time)
    {
        skillCD = skillMaxCD;
        rb.AddForce(-transform.forward * force);
        yield return new WaitForSeconds(time);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        yield return null;
    }

    void Attack()
    {
        var rotationVector = transform.rotation.eulerAngles;
        if (ammo > 0)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rotationVector.y = 0;
                transform.rotation = Quaternion.Euler(rotationVector);
                if (playerScript.attacking == false)
                    StartCoroutine(BasicAttack());
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rotationVector.y = 180;
                transform.rotation = Quaternion.Euler(rotationVector);
                if (playerScript.attacking == false)
                    StartCoroutine(BasicAttack());
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rotationVector.y = 270;
                transform.rotation = Quaternion.Euler(rotationVector);
                if (playerScript.attacking == false)
                    StartCoroutine(BasicAttack());
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rotationVector.y = 90;
                transform.rotation = Quaternion.Euler(rotationVector);
                if (playerScript.attacking == false)
                    StartCoroutine(BasicAttack());
            }
        }

        Reload2(reloadCD);

    }

    void Reload2(float reloadT)
    {
        if (Input.GetKeyDown(KeyCode.R) && reloadT <= 0)
        {
            ammo = maxammo;
            reloadCD = 1.75f;
        }
    }

    IEnumerator BasicAttack()
    {
        playerScript.attacking = true;
        reloadCD = 1.75f;
        Instantiate(weapon, attackPoint.position, attackPoint.rotation);
        ammo--;
        yield return new WaitForSeconds(attackRate);
        playerScript.attacking = false;
        yield return null;
    }

    void UIStuff()
    {
        ammoStuff.transform.GetChild(2).GetComponent<Text>().text = ammo.ToString();
        ammoStuff.transform.GetChild(3).GetComponent<Text>().text = maxammo.ToString();
        cooldownIndicator.fillAmount = 1 - (skillCD / skillMaxCD);
        if (reloadCD >= 1)
            ammoStuff.transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = 1;
        if (reloadCD < 1)
            ammoStuff.transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = reloadCD;
    }
}
