using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] Rigidbody bullet;
    [SerializeField] Transform firePos;
    [SerializeField] float projSpeed = 1500f;
    [SerializeField] float fireRate = 0.5f;
    float frCounter;
    [SerializeField] Vector2 fireSpreadAmplitude;
    [SerializeField] float sprintSpeed;
    [SerializeField] Vector3 sprintRot;
    [SerializeField] Vector3 dashPos;

    [Header("FX")]
    [SerializeField] Animator anim;
    [SerializeField] Animation shootAnim;
    [SerializeField] AnimationClip shootClip;
    [SerializeField] AudioClip shootSound;
    [SerializeField] ParticleSystem muzzleFlash;

    bool sprinting;
    bool moving;
    bool stillHolding;

    Vector3 targetRot;
    Quaternion sprintRotRes;
    float oldSprintSpeed;

    Vector3 oldPos;
    CCImpact cci;
    Vector3 targetPos;
    
    void Start ()
    {
        CursorManager.LockCursor();
        oldSprintSpeed = PlayerInstance.instance.GetComponent<FirstPersonController>().movementSpeed;
        oldPos = transform.localPosition;
        cci = PlayerInstance.instance.GetComponent<CCImpact>();
    }

    void Update ()
    {
        Animation();
        Shooting();
        Sprinting();
        Dashing();
    }

    void Shooting () 
    {
        frCounter += Time.deltaTime;
        if (Input.GetMouseButton(0) && frCounter >= fireRate && !sprinting) 
        {
            ShootProj();
            muzzleFlash.Play();
            frCounter = 0f;
        }
    }

    void ShootProj () 
    {
        frCounter = 0f;

        var b = Instantiate(bullet, firePos.position, firePos.rotation);
        b.AddForce(projSpeed * b.transform.forward);
        Destroy(b.gameObject, 10f);

        firePos.localEulerAngles = CalculateSpread();

        shootAnim.Rewind(shootClip.name);
        shootAnim.Play(shootClip.name);
        GetComponent<AudioSource>().PlayOneShot(shootSound);
    }

    Vector3 CalculateSpread () 
    {
        Vector3 spread = new Vector3(0f + Random.Range(-fireSpreadAmplitude.x, fireSpreadAmplitude.x), //x
                                     0f + Random.Range(-fireSpreadAmplitude.y, fireSpreadAmplitude.y), //y
                                     0f);                                                              //z

        return spread;
    }
    
    void Animation () 
    {
        moving = (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) && !cci.dashing && cci.cc.isGrounded;
        anim.SetBool("Walking", moving);
    }

    void Sprinting () 
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && moving && !Input.GetMouseButton(0)) 
            sprinting = true;
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0 || cci.dashing || Input.GetMouseButton(0))
            sprinting = false;


        if (sprinting || stillHolding) 
        {
            sprintRotRes = Quaternion.Euler(sprintRot);
            PlayerInstance.instance.GetComponent<FirstPersonController>().movementSpeed = sprintSpeed;
            anim.speed = 1.25f;
        }

        else 
        {
            sprintRotRes = Quaternion.Euler(Vector3.zero);
            PlayerInstance.instance.GetComponent<FirstPersonController>().movementSpeed = oldSprintSpeed;
            anim.speed = 1f;
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, sprintRotRes, (Time.deltaTime * 10f));
    }

    void Dashing () 
    {
        targetPos = (cci.dashing) ? dashPos : oldPos;
        transform.localPosition = Vector3.Slerp(transform.localPosition, targetPos, 8f * Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetKeyDown(KeyCode.LeftControl) && input.magnitude > 0 && sprinting) 
            cci.AddImpact(PlayerInstance.instance.transform.TransformDirection(input), 300f);
    }
}
