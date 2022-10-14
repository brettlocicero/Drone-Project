using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] float tiltFactor;
    [SerializeField] float maxSpeed;
    [SerializeField] Transform droneObj;

    Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        Vector3 tiltDir = new Vector3(-(rb.velocity.z / maxSpeed) * tiltFactor, 0f, (rb.velocity.x / maxSpeed) * tiltFactor);
        Quaternion res = Quaternion.Euler(tiltDir);
		droneObj.localRotation = Quaternion.Slerp(droneObj.localRotation, res, (Time.deltaTime * 5f));
    }
}
