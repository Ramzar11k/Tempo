using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    Transform target;

    Vector3 offset;
    Vector3 targetCamPos;

    void Start()
    {
        transform.LookAt(target);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - target.position;
    }

    private void Update()
    {
    }

    void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        if (target != null)
            targetCamPos = target.position + offset;
        transform.position = targetCamPos;
    }
}
