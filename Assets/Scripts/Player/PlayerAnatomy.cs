//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntBoolEvent : UnityEvent<int, bool> {}

[System.Serializable]
public class ArmorPartCheck
{
    public bool helmet;
    public bool breastplate;
    public bool rightPauldron;
    public bool leftPauldron;
    public bool rightArm;
    public bool leftArm;
    public bool tasset;
    public bool rightBoot;
    public bool leftBoot;

    public bool[] all => new bool[]
    {
        helmet,
        breastplate,
        rightPauldron,
        leftPauldron,
        rightArm,
        leftArm,
        tasset,
        rightBoot,
        leftBoot
    };
}

public class PlayerAnatomy : MonoBehaviour
{
    public enum LookType
    {
        skin,
        muscle,
        skeleton
    }

    public ArmorPartCheck displayArmor;
    public LookType lookType;
    [Header("Dependencies")]
    public List<GameObject> parts = new List<GameObject>();
    public RuntimeAnimatorController skinController;
    public RuntimeAnimatorController muscleController;
    public RuntimeAnimatorController skeletonController;

    IntBoolEvent onArmorDisplayChange = new IntBoolEvent();
    List<SpriteRenderer> partRenderers = new List<SpriteRenderer>();
    SpriteRenderer bodyRenderer;
    Animator bodyAnimator;
    LookType _lookType
    {
        get
        {
            return lookType;
        }
        set
        {
            value = lookType;
            if(bodyAnimator == null)
                bodyAnimator = transform.GetChild(0).GetComponent<Animator>();
            
            if(lookType == LookType.skin)
                bodyAnimator.runtimeAnimatorController = skinController;
            else if (lookType == LookType.muscle)
                bodyAnimator.runtimeAnimatorController = muscleController;
            else if (lookType == LookType.skeleton)
                bodyAnimator.runtimeAnimatorController = skeletonController;

        }
    }

    void Start()
    {
        bodyRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        foreach (Transform partTransform in transform.GetChild(0))
        {
            partRenderers.Add(partTransform.GetComponent<SpriteRenderer>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && parts.Count > 0)
            LosePart();
        
        UpdateArmorDisplay();
    }

    private void UpdateArmorDisplay()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            partRenderers[i].enabled = displayArmor.all[i];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Spikes>())
            LosePart();
    }

    void LosePart()
    {
        GameObject part = parts[Random.Range(0, parts.Count)];
        Rigidbody2D partBody = part.GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.Range(-5, 5), Random.Range(1, 5));
        part.transform.SetParent(null);
        partBody.isKinematic = false;
        partBody.AddTorque(20f, ForceMode2D.Impulse);
        partBody.AddForce(direction, ForceMode2D.Impulse);
        parts.Remove(part);
    }
}
