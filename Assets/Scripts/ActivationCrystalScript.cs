using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationCrystalScript : MonoBehaviour {

    public bool active;
    public bool toggleable;
    Color activated;
    Color deactivated;

	void Start ()
    {
        activated = transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material.color;
        deactivated = transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if (active == false)
            {
                for (var i = 0; i < transform.GetChild(0).childCount - 1; i++)
                {
                    transform.GetChild(0).GetChild(i + 1).GetComponent<Renderer>().material.color = activated;
                    active = true;
                }
            }
            else if (active == true && toggleable == true)
                for (var i = 0; i < transform.GetChild(0).childCount - 1; i++)
                {
                    transform.GetChild(0).GetChild(i + 1).GetComponent<Renderer>().material.color = deactivated;
                    active = false;
                }

            if (other.transform.parent == null)
                Destroy(other.gameObject);
        }
        if (other.CompareTag("Projectile"))
            Destroy(other.gameObject);
    }
}
