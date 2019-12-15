using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {
    

    void Start ()
    {
		
	}

	void Update () {
		
	}

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            ProjectileScript projectileScript = other.GetComponent<ProjectileScript>();
            if (projectileScript.destroyable == true)
                Destroy(other.gameObject);
            if (projectileScript.reflectable == true)
            {
                projectileScript.Redirect();
                other.transform.rotation = Quaternion.Euler(transform.parent.rotation.eulerAngles.x - 90f, transform.parent.rotation.eulerAngles.y, transform.parent.rotation.eulerAngles.z);
                other.tag = "Weapon";
            }
        }
    }
}
