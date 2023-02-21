using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] int health = 50;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Rigidbody rb;

    Vector3 dir, lookAt;

    void Start () 
    {
        target = PlayerInstance.instance.transform;
    }

    void FixedUpdate ()
    {
        Move();
    }

    void Move ()
    {
        dir = (target.position - transform.position).normalized;
        lookAt = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookAt);

        rb.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage (int dmg) 
    {
        health -= dmg;

        if (health <= 0f)
            Destroy(gameObject);
    }
}
