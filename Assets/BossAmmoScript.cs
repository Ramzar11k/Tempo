using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAmmoScript : MonoBehaviour {

    public bool shot;
    public float chargeSpeed = 0.5f;
    float speed = 10f;
    public int damage;
    public Transform firePoint;
    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (shot == false)
            Charge();
        else
            Fire();
    }

    void Charge()
    {
        transform.position = firePoint.position;
        transform.LookAt(player);
        transform.localScale += new Vector3(chargeSpeed * Time.deltaTime, chargeSpeed * Time.deltaTime, chargeSpeed * Time.deltaTime);
    }

    void Fire()
    {
        transform.parent = null;
        Destroy(gameObject, 3f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            other.GetComponent<PlayerScript>().health -= damage;
        }
    }
}
