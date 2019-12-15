using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour {

    int regen = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerScript>().health + regen > other.GetComponent<PlayerScript>().maxHealth)
                other.GetComponent<PlayerScript>().health = other.GetComponent<PlayerScript>().maxHealth;
            else
                other.GetComponent<PlayerScript>().health += regen;
            Destroy(gameObject);
        }
    }
}
