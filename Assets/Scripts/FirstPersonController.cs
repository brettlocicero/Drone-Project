using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    /// <summary>
    /// Move the player charactercontroller based on horizontal and vertical axis input
    /// </summary>

    public bool directionChangeMidair;

    [HideInInspector] public float yVelocity = 0f;
    [HideInInspector] public float zVelocity = 0f;
    [Range(-5f, -25f)]
    public float gravity = -15f;
    //the speed of the player movement
    [Range(5f, 1000f)]
    public float movementSpeed = 10f;
    //jump speed
    [Range(5f, 150f)]
    public float jumpSpeed = 10f;

    //now the camera so we can move it up and down
    public Transform cameraTransform;
    float pitch = 0f;
    [Range(1f, 90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;
    [Range(0.5f, 5f)]
    public float mouseSensitivity = 2f;

    //the charachtercompononet for moving us
    CharacterController cc;
    [SerializeField] CCImpact cci;

    [Header("Footstep Stuff")]
    public bool use;
    public AudioClip[] footsteps;
    public float footstepInterval = 0.5f;
    float footstepCounter;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Move();

        footstepCounter += Time.deltaTime;
    }

    void Look ()
    {
        //get the mouse inpuit axis values
        float xInput = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float yInput = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        //turn the whole object based on the x input
        transform.Rotate(0, xInput, 0);
        //now add on y input to pitch, and clamp it
        pitch -= yInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        //create the local rotation value for the camera and set it
        Quaternion rot = Quaternion.Euler(pitch, 0, 0);
        cameraTransform.localRotation = rot;
    }

    void Move ()
    {
        //update speed based onn the input
        //if (cci.dashing) return;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        //transform it based off the player transform and scale it by movement speed
        Vector3 move = input * movementSpeed;

        //is it on the ground
        if (cc.isGrounded)
        {
            //check for jump here
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
            }

            if (input.x != 0 || input.z != 0) Footstep();
        }
        //now add the gravity to the yvelocity
        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;
        move.z = move.z - zVelocity;
        //and finally move
        cc.Move(transform.TransformDirection(move) * Time.deltaTime);
    }

    public IEnumerator LaunchInAir (float launchScalar, float stunTime)
    {
        yVelocity = launchScalar;
        zVelocity = launchScalar;
        float cachedMoveSpeed = movementSpeed;
        movementSpeed = 0;
        yield return new WaitForSeconds (stunTime);
        zVelocity = 0;
        movementSpeed = cachedMoveSpeed;
    }

    public void Footstep ()
    {
        if (cc.isGrounded && use && footstepCounter >= footstepInterval) 
        {
            GetComponent<AudioSource>().PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
            footstepCounter = 0f;
        }
    }
}