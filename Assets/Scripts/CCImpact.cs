using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCImpact : MonoBehaviour
{
    [SerializeField] float ccMass = 3f;
    [SerializeField] Animator dashScreen;
    public bool dashing;
    Vector3 impact = Vector3.zero;
    CharacterController cc;

    float playerHeight;

    void Start ()
    {
        cc = GetComponent<CharacterController>();
    }

    public void AddImpact (Vector3 dir, float force) 
    {
        dir.Normalize();
        //dashScreen.SetTrigger("TakeDamage");

        if (dir.y < 0) 
            dir.y = -dir.y;

        impact += dir.normalized * force / ccMass;
    }

    void Update ()
    {
        if (impact.magnitude > 5f) 
        {
            cc.Move(impact * Time.deltaTime);
            playerHeight = 0.5f;
            dashing = true;
        }

        else 
        {
            playerHeight = 2f;
            dashing = false;
        }

        impact = Vector3.Lerp(impact, Vector3.zero, 4f * Time.deltaTime);
        cc.height = Mathf.Lerp(cc.height, playerHeight, 4f * Time.deltaTime);
    }
}