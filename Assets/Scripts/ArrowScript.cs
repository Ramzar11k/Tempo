using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    float speed = 5f;
	
	void Start ()
    {
        Destroy(gameObject, 1.5f);	
	}
	
	void Update ()
    {
        transform.Translate(0, Time.deltaTime * speed, 0);	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
