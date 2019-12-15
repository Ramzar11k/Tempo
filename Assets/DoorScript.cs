using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    Vector3 open;
    Vector3 closed;

	void Start ()
    {
        closed = transform.position;
        open = transform.position + new Vector3(0, 3, 0);        	
	}

    public void Open()
    {
        transform.position = Vector3.Lerp(transform.position, open, 0.05f);
    }

    public void Close()
    {
        transform.position = Vector3.Lerp(transform.position, closed, 0.05f);
    }
}
