using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public int damage;
    public float speed;
    public float duration;

    public bool reflectable;
    public bool destroyable;

	void Start ()
    {
        transform.SetParent(null);
        Invoke("DestroyMe", duration);	
	}
	
	void Update ()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void Redirect()
    {
        CancelInvoke("DestroyMe");
        Invoke("DestroyMe", duration);
        GetComponent<SphereCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerScript>().health -= damage;
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy") && CompareTag("Weapon"))
            Destroy(gameObject);

        if (other.CompareTag("Projectile") && CompareTag("Weapon"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
