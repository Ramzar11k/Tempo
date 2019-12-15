using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {
    
	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 2f);
        if (name.Length > 18)
        {
            gameObject.tag = "Weapon";
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
    }
	

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && CompareTag("Weapon"))
            Destroy(gameObject);
    }
}
