using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float tiltFactor;
    [SerializeField] float maxSpeed;
    [SerializeField] float elusiveness = 50f;
    [SerializeField] Transform droneObj;
    [SerializeField] float stoppingDist;
    [SerializeField] float hoverHeight = 10f;

    Rigidbody rb;
    Vector3 dir;
    float dist, force; 

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        Vector3 tiltDir = new Vector3(-(rb.velocity.z / maxSpeed) * tiltFactor, 0f, (rb.velocity.x / maxSpeed) * tiltFactor);
        Quaternion res = Quaternion.Euler(tiltDir);
		droneObj.localRotation = Quaternion.Slerp(droneObj.localRotation, res, (Time.deltaTime * 5f));
        if (target) transform.LookAt(target);
    }

    void FixedUpdate () 
    {
        if (!target) return;
        
        dist = Vector3.Distance(transform.position, target.position);
        if (dist < stoppingDist) return;

        force = (dist * elusiveness);
        dir = (target.position - transform.position).normalized * force;        
        rb.velocity = new Vector3(dir.x, 0f, dir.z);
    }
}
