using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private float launchThreshold;
    Animator bodyAnimator;
    SpriteRenderer bodyRenderer;
    Rigidbody2D myRigidbody;
    Player player;
    Stamina stamina;

    private void Awake()
    {
        player = GetComponent<Player>();    
        stamina = GetComponent<Stamina>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Transform body = transform.GetChild(0);
        bodyAnimator = body.GetComponent<Animator>();    
        bodyRenderer = body.GetComponent<SpriteRenderer>();
        player.onJump.AddListener(TriggerJump);
    }

    private void TriggerJump()
    {
        bodyAnimator.SetTrigger("OnJump");
        bodyAnimator.SetBool("Launching", true);
    }

    // Update is called once per frame
    void Update()
    {
        bodyAnimator.SetInteger("MoveX", (int)player.moveDirection.x);
        bodyAnimator.SetBool("Grounded",player.isGrounded);
        bodyRenderer.flipX = player.lastDirection.x != 1;
        bodyAnimator.SetFloat("VelocityY", myRigidbody.velocity.y);

        if(bodyAnimator.GetBool("Launching")
                && myRigidbody.velocity.y < launchThreshold)
        {
            bodyAnimator.SetBool("Launching", false);
        }
    }
}
