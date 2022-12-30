using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float crouchSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] Animator anim;

    CharacterController controll;
    float turnSmoothVelocity;

    private void Awake()
    {
        controll = GetComponent<CharacterController>();
    }

    //Dislike that it checks horizontal and vertical input and then creates a vector3 when no
    //input has been made
    // Update is called once per frame
    void Update()
    {
        //Stop all movement if a dialogue is playing
        if (DialogueManager.GetInstance().dialogueIsPlaying ||
            TransitionManager.GetInstance().IsTeleporting)
        {
            anim.SetBool("isJogging", false);
            anim.SetBool("isCrouching", false);
            return;
        }


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            anim.SetBool("isJogging", true);
        }

        else
        {
            anim.SetBool("isJogging", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            if (anim.GetBool("isCrouching"))
            {
                anim.SetBool("isCrouching", false);
            }

            else
            {
                anim.SetBool("isCrouching", true);
            }
            
        }
        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDir = Vector3.zero;

        if (!controll.isGrounded) { 

            moveDir += Physics.gravity; 
        } else {

            moveDir += Vector3.down;
        }  

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir += Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
        }

        if (anim.GetBool("isCrouching"))
        {
            controll.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
        }
        else
        {
            controll.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        
    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

}
