using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public bool isLanding;
    [SerializeField] private float launchThreshold;
    [SerializeField] private GameObject runningSweatObject;
    Animator bodyAnimator;
    SpriteRenderer bodyRenderer;
    SpriteRenderer sweatObjectRenderer;
    Rigidbody2D myRigidbody;
    PlayerMovement player;
    AnimatorStateInfo animationInfo;
    PlayerBoneForm boneForm;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();    
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Transform body = transform.GetChild(0);
        bodyAnimator = body.GetComponent<Animator>();    
        bodyRenderer = body.GetComponent<SpriteRenderer>();
        sweatObjectRenderer = runningSweatObject.GetComponent<SpriteRenderer>();
        player.onJump.AddListener(TriggerJump);
        boneForm = GetComponentInChildren<PlayerBoneForm>();

        if(boneForm != null)
        {
            boneForm.onExplode.AddListener(HideBody);
            boneForm.onReform.AddListener(ShowBody);
        }

    }

    private void HideBody()
    {
        bodyRenderer.enabled = false;
    }

    private void ShowBody()
    {
        bodyRenderer.enabled = true;
    }

    private void TriggerJump()
    {
        bodyAnimator.SetTrigger("OnJump");
        bodyAnimator.SetBool("Launching", true);
    }

    // Update is called once per frame
    void Update()
    {
        animationInfo = bodyAnimator.GetCurrentAnimatorStateInfo(0);
        isLanding = animationInfo.IsName("playerHeavyLand") && animationInfo.IsName("playerLightLand");
        bodyAnimator.SetInteger("MoveX", (int)player.moveDirection.x);
        bodyAnimator.SetBool("Grounded",player.isGrounded);
        bodyRenderer.flipX = player.lastDirection.x != 1;
        sweatObjectRenderer.flipX = player.lastDirection.x != 1;
        bodyAnimator.SetFloat("VelocityY", myRigidbody.velocity.y);
        bodyAnimator.speed = player.isRunning ? 1f : 1.6f;

        if(bodyAnimator.GetBool("Launching")
                && myRigidbody.velocity.y < launchThreshold)
        {
            bodyAnimator.SetBool("Launching", false);
        }

        if(myRigidbody.velocity.y < player.currentMaxVelocity.y && !bodyAnimator.GetBool("MaxVelocityYReached"))
            bodyAnimator.SetBool("MaxVelocityYReached", true);
        else if (myRigidbody.velocity.y >= player.currentMaxVelocity.y && bodyAnimator.GetBool("MaxVelocityYReached"))
            bodyAnimator.SetBool("MaxVelocityYReached", false);

        if(player.isRunning && !runningSweatObject.activeInHierarchy)
            runningSweatObject.SetActive(true);
        else if(!player.isRunning && runningSweatObject.activeInHierarchy)
            runningSweatObject.SetActive(false);
    }
}
