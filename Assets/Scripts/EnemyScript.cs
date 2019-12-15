using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public bool damaged;
    bool invulnerable;

    PlayerScript playerScript;

    public GameObject healthDrop;

    List<Color> componentColors = new List<Color>();

    public int health = 100;
    public int damage;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        for (var i = 0; i < transform.GetChild(0).childCount; i++)
        {
            componentColors.Add(transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().material.color);
        }
    }

    private void Update()
    {
        Death();
    }

    IEnumerator DamagedRoutine()
    {
        damaged = true;
        invulnerable = true;
        health -= playerScript.damage;
        for (var i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponent<Renderer>().material.color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);
        for (var i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponent<Renderer>().material.color = componentColors[i];
        }
        invulnerable = false;
        yield return null;
    }

    void Death()
    {
        if (health <= 0)
        {
            var x = Random.Range(0, 101);
            if (x < 60)
                Instantiate(healthDrop, transform.position + new Vector3(0,0.2f,0), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if (invulnerable == false)
                StartCoroutine(DamagedRoutine());
        }
    }
}
