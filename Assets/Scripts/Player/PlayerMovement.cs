using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private KeyCode jumpInput;
    [SerializeField] private KeyCode runInput;
    [SerializeField] private LayerMask levelTiles;
    [SerializeField] private float walkMoveForce;
    [SerializeField] private float runMoveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundGravity;
    [SerializeField] private float jumpGravity;
    [SerializeField] private Vector2 walkMaxVelocity;
    [SerializeField] private Vector2 runMaxVelocity;
    public float maxStamina;
    [Tooltip("Time (s) to lose 1 stamina")]
    [SerializeField]  private float staminaDecayTime;
    [Tooltip("Time (s) to regain 1 stamina")]
    [SerializeField]  private float staminaRegenTime;
    [HideInInspector] public UnityEvent onJump = new UnityEvent();
    float staminaCounter;
    
    [Header("Read-Only")]
    public Vector2 currentMaxVelocity;
    public Vector2 moveDirection;
    public Vector2 lastDirection;
    public float currentMoveForce;
    public float currentStamina;
    public bool canMove;
    public bool isGrounded;
    public bool isLanding;
    public bool isRunning;

    /*Dependencies*/
    Rect groundedArea;
    PlayerAnimator playerAnimator;
    Collider2D myCollider;
    Rigidbody2D myRigidbody;
    PlayerBoneForm boneForm;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        currentStamina = maxStamina;
        boneForm = GetComponentInChildren<PlayerBoneForm>();
        if(boneForm != null)
        {
            boneForm.onReform.AddListener(EnableMove);
            boneForm.onExplode.AddListener(DisableMove);
        }
    }

    private void EnableMove()
    {
        canMove = true;
    }

    private void DisableMove()
    {
        canMove = false;
    }

    void Update()
    {
        UpdateIsGrounded();
        UpdateGravity();
        Move();
        UpdateRunning();
        JumpCheck();
    }

    private void UpdateIsGrounded()
    {
        groundedArea = new Rect((Vector2)myCollider.bounds.min + Vector2.down * 0.01f,
                new Vector2(myCollider.bounds.size.x, 0.01f));
        groundedArea.xMin += 0.15f;
        groundedArea.xMax -= 0.15f;
        isGrounded = Physics2D.OverlapArea(groundedArea.min, groundedArea.max, levelTiles);
    }

    private void UpdateGravity()
    {
        myRigidbody.gravityScale = isGrounded ? groundGravity : jumpGravity;
    }

    private void Move()
    {
        if(playerAnimator.isLanding) 
            return;
        
        moveDirection = new Vector2(canMove ? Input.GetAxisRaw("Horizontal") : 0f, 0f);

        if(moveDirection.x == 0 && isGrounded)
        {
            myRigidbody.velocity = new Vector2(
                    myRigidbody.velocity.x * 0.8f,
                    myRigidbody.velocity.y);
            return;
        }

        currentMoveForce = isRunning ? runMoveForce : walkMoveForce;
        currentMaxVelocity = isRunning ? runMaxVelocity : walkMaxVelocity;

        myRigidbody.AddForce( moveDirection * currentMoveForce, ForceMode2D.Force);
        myRigidbody.velocity = new Vector2(
                Mathf.Clamp(myRigidbody.velocity.x, -currentMaxVelocity.x, currentMaxVelocity.x),
                Mathf.Clamp(myRigidbody.velocity.y, -currentMaxVelocity.y, currentMaxVelocity.y));
                
        if(moveDirection.x != 0)
            lastDirection = moveDirection;
    }

    private void JumpCheck()
    {
        if(isGrounded && canMove && Input.GetKeyDown(jumpInput) && boneForm == null)
        {
            onJump.Invoke();
            myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        if (myCollider == null)
            myCollider = GetComponent<Collider2D>();
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(groundedArea.center, new Vector2(
                groundedArea.size.x, groundedArea.size.y));
    }

    private void UpdateRunning()
    {
        if(Input.GetKey(runInput) && isGrounded && moveDirection.x != 0)
        {
            isRunning = true;
            if(currentStamina <= 0)
                return;    

            staminaCounter += Time.deltaTime;
            if(staminaCounter > staminaDecayTime)
            {
                staminaCounter = 0;
                currentStamina--;
            }
        } else {
            if(isRunning)
            {
                isRunning = false;
                staminaCounter = 0;
            }

            if(currentStamina < maxStamina)
            {
                staminaCounter += Time.deltaTime;
                if(staminaCounter > staminaRegenTime)
                {
                    staminaCounter = 0;
                    currentStamina++;
                }
            }
        }
    }
}
