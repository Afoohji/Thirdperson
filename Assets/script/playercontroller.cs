using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Camera followCamera;

    [SerializeField] private float rotationspeed = 10f;

    private Vector3 playervelocity;
    [SerializeField] private float gravityvalue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpheight = 2.5f;

    public Animator animator;

    public static playercontroller instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (CheckWinner.instance.isWinner)
        {
            case true:
                animator.SetBool("Victory", CheckWinner.instance.isWinner);
                break;
            case false:
                Movement();
                break;

        }
        
    }

    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        if(characterController.isGrounded && playervelocity.y < -2f)
        {
            playervelocity.y = -1f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0)* new Vector3 (horizontalInput,0, verticalInput);

        Vector3 movementDirection = movementInput.normalized;

        characterController.Move(movementDirection * playerSpeed * Time.deltaTime);

        if(movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
           transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation,rotationspeed * Time.deltaTime);
        }

        if(Input.GetButtonDown("Jump")&& groundedPlayer)
        {
            playervelocity.y += Mathf.Sqrt(jumpheight * -3f * gravityvalue);
            animator.SetTrigger("Jumping");
        }

        playervelocity.y += gravityvalue * Time.deltaTime;
        characterController.Move(playervelocity * Time.deltaTime);

        animator.SetFloat("speed",Mathf.Abs(movementDirection.x)+Mathf.Abs(movementDirection.z));
        animator.SetBool("ground", characterController.isGrounded);
        
    }
}
