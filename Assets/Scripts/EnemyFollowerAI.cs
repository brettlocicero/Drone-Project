using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowerAI : MonoBehaviour
{
    [SerializeField] float health = 50f;
    [SerializeField] Transform target;
    public Rigidbody rb;
    [SerializeField] float moveSpeed = 4f;

    [Header("Attack Stats")]
    [SerializeField] bool inAttack;
    [SerializeField] float attackSpeed = 2f, stopRange = 2f, turnSpeed = 2f;
    [SerializeField] bool drawAttackRadius;
    [SerializeField] float damageAmount = 8f, attackRadius = 2f;
    [SerializeField] Transform attackCenter;
    [SerializeField] LayerMask playerLayer;

    [Header("FX")]
    [SerializeField] GameObject deathFX;
    [SerializeField] bool setRandomPitch = true;
    [SerializeField] Vector2 randPitch;
    [SerializeField] Animator anim;
    [SerializeField] float camShakeAmount = 5f, crowdNoiseFactor = 0.05f;
    [SerializeField] AudioClip attackSound;
    [SerializeField] bool randSize;
    [SerializeField] Vector2 randSizeRange;

    void Start() 
    {
        target = PlayerInstance.instance.transform;
        rb = GetComponent<Rigidbody>();

        if (setRandomPitch)
            GetComponent<AudioSource>().pitch = Random.Range(randPitch.x, randPitch.y);

        if (randSize) 
        {
            float scale = Random.Range(randSizeRange.x, randSizeRange.y);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    
    void FixedUpdate()
    {
        Vector3 dir = new Vector3(target.position.x - transform.position.x, target.position.y, target.position.z - transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= stopRange && !inAttack)
            StartCoroutine(BeginAttack());

        else if (!inAttack) 
            rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * moveSpeed);

        if (inAttack) 
        {
            anim.SetBool("Attacking", true);
            anim.SetBool("Walking", false);
        }

        else 
        {
            anim.SetBool("Attacking", false);
            anim.SetBool("Walking", true);
        }

        //failsafe
        if (transform.position.y < -100f) 
        {
            WaveManager.instance.RemoveEnemyFromList(transform);
            Destroy(gameObject);
        }
    }

    IEnumerator BeginAttack () 
    {
        inAttack = true;
        GetComponent<AudioSource>().PlayOneShot(attackSound);
        yield return new WaitForSeconds(attackSpeed);
        inAttack = false;
    }

    public void Attack () 
    {
        Collider[] hitObjs = Physics.OverlapSphere(attackCenter.position, attackRadius, playerLayer);

        //if (hitObjs.Length > 0) 
            //target.GetComponent<PlayerInstance>().TakeDamage(damageAmount);
    }

    void OnDrawGizmos()
    {
        if (drawAttackRadius) 
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
            Gizmos.DrawSphere(attackCenter.position, attackRadius);
        }
    }

    public void TakeDamage (float dmg)
    {
        health -= dmg;

        if (health <= 0f)
        {
            var d = Instantiate(deathFX, transform.position, transform.rotation);
            d.transform.localScale = transform.localScale;
            Destroy(d, 10f);

            //CinemachineShake.instance.ShakeCamera(camShakeAmount, 0.5f, 50f);
            //target.GetComponent<PlayerInstance>().AddMoney(Random.Range(5, 10));
            WaveManager.instance.RemoveEnemyFromList(transform);
            Destroy(gameObject);
        }
    }
}