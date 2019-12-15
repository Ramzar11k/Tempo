using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public float speed = 10f;
    public bool attacking;

    public int damage = 3;
    public int maxHealth = 10;
    public int health = 10;

    Rigidbody playerRB;
    Animator anim;

    GameObject UI;
    Slider healthBar;

	void Start ()
    {
        anim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();
        UI = GameObject.FindGameObjectWithTag("UI").gameObject;
        healthBar = UI.transform.GetChild(0).GetChild(1).GetComponent<Slider>();
	}

    private void Update()
    {
        healthBar.value = ((float)health / maxHealth) * 100;
        if (health <= 0)
        {
            UI.transform.GetChild(2).gameObject.SetActive(true);
            healthBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void FixedUpdate ()
    {
        PlayerMovement();
        WalkAnimation();
	}

    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(0, 0, speed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(0, 0, -speed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(-speed * Time.deltaTime, 0, 0, Space.World);
    }

    void WalkAnimation()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            anim.SetBool("isMoving", true);
        else
            anim.SetBool("isMoving", false);
    }
    
}
