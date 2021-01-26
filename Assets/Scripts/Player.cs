using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public Vector2 maxVelocity;
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundGravity;
    [SerializeField] private float jumpGravity;
    [SerializeField] private float gravityPerArmorPart;
    public float runMultiplier;
    public bool isGrounded;
    public bool isLanding;
    [SerializeField] private LayerMask levelTiles;

    Collider2D myCollider;
    Rigidbody2D myRigidbody;
    SpriteRenderer bodyRenderer;
    AnatomyControl anatomy;
    Stamina stamina;
    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public Vector2 lastDirection;
    Vector2 groundedMin;
    Vector2 groundedMax;
    Vector2 fixedVelocity;
    PlayerAnimator playerAnimator;
    int armorParts;

    [HideInInspector] public UnityEvent onJump = new UnityEvent();


    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        anatomy = GetComponent<AnatomyControl>();
        myRigidbody = GetComponent<Rigidbody2D>();
        stamina = GetComponent<Stamina>();
    }

    void Start()
    {
        bodyRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        UpdateIsGrounded();
        UpdateGravity();
        Move();
        JumpCheck();
    }

    private void UpdateIsGrounded()
    {
        armorParts = anatomy.parts.Count;
        groundedMin = new Vector2(myCollider.bounds.center.x - (myCollider.bounds.extents.x * 4) / 5,
                myCollider.bounds.center.y - myCollider.bounds.extents.y - 0.1f);
        groundedMax = new Vector2(myCollider.bounds.center.x + (myCollider.bounds.extents.x * 4) / 5,
                myCollider.bounds.center.y - myCollider.bounds.extents.y);

        isGrounded = Physics2D.OverlapArea(groundedMin, groundedMax, levelTiles);
    }

    private void UpdateGravity()
    {
        myRigidbody.gravityScale = isGrounded ?
                groundGravity + gravityPerArmorPart * armorParts :
                jumpGravity + gravityPerArmorPart * armorParts;
    }

    private void Move()
    {
        if(playerAnimator.isLanding) 
            return;
            
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        myRigidbody.AddForce( moveDirection * moveForce * runMultiplier
                * (stamina.running ? runMultiplier : 1f),
                ForceMode2D.Force);

        fixedVelocity = new Vector2(
                Mathf.Clamp(myRigidbody.velocity.x, -maxVelocity.x * (stamina.running ? runMultiplier : 1f), maxVelocity.x * (stamina.running ? runMultiplier : 1f)),
                Mathf.Clamp(myRigidbody.velocity.y, -maxVelocity.y * (stamina.running ? runMultiplier : 1f), maxVelocity.y * (stamina.running ? runMultiplier : 1f)));
        
        if(isGrounded && moveDirection.x == 0)
            fixedVelocity *= 0.2f;

        myRigidbody.velocity = fixedVelocity;
        
        if(moveDirection.x != 0)
            lastDirection = moveDirection;
    }

    private void JumpCheck()
    {
        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
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
        Vector2 groundedCenter = new Vector2(
            (groundedMin.x + groundedMax.x) / 2,
            (groundedMin.y + groundedMax.y) / 2);
        Gizmos.DrawCube(groundedCenter, new Vector2(groundedMax.x - groundedMin.x, groundedMax.y - groundedMin.y));
    }
}
