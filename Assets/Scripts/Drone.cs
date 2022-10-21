using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] Transform target;

    [Header("Attack Settings")]
    [SerializeField] Transform[] fireSpots;
    [SerializeField] Rigidbody proj;
    [SerializeField] float projForce = 3000f;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] Vector2 fireSpreadAmplitude;
    float counter;

    [Header("Movement")]
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

        if (!target) return;

/////// ONLY THINGS THAT DO NOT USE TARGET BELOW
        
        transform.LookAt(target);
        FireProjController();
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

    void FireProjController () 
    {
        counter += Time.deltaTime;

        if (counter >= fireRate) 
        {
            Transform fireSpot = fireSpots[Random.Range(0, fireSpots.Length)];
            fireSpot.localEulerAngles = CalculateSpread();
            
            Rigidbody p = Instantiate(proj, fireSpot.position, fireSpot.rotation);
            p.AddForce(projForce * p.transform.forward);

            counter = 0f;
        }        
    }

    Vector3 CalculateSpread () 
    {
        Vector3 spread = new Vector3(0f + Random.Range(-fireSpreadAmplitude.x, fireSpreadAmplitude.x), //x
                                     0f + Random.Range(-fireSpreadAmplitude.y, fireSpreadAmplitude.y), //y
                                     0f);                                                              //z

        return spread;
    }
}
