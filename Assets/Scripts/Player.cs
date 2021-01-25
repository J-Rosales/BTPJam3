using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxVelocity;
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundGravity;
    [SerializeField] private float jumpGravity;
    [SerializeField] private float gravityPerArmorPart;
    public bool isGrounded;
    [SerializeField] private LayerMask levelTiles;

    Collider2D myCollider;
    Rigidbody2D myRigidbody;
    SpriteRenderer bodyRenderer;
    AnatomyControl anatomy;
    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public Vector2 lastDirection;
    Vector2 groundedMin;
    Vector2 groundedMax;
    float fixedVelocity;
    int armorParts;

    [HideInInspector] public UnityEvent onJump = new UnityEvent();


    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        anatomy = GetComponent<AnatomyControl>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        bodyRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        myRigidbody.AddForce(moveDirection * moveForce, ForceMode2D.Force);
        fixedVelocity = Mathf.Clamp(myRigidbody.velocity.x, -maxVelocity, maxVelocity);
        
        if(isGrounded && moveDirection.x == 0)
            fixedVelocity *= 0.1f;

        myRigidbody.velocity = new Vector2(fixedVelocity, myRigidbody.velocity.y);
        
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
