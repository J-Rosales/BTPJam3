using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBoneForm : MonoBehaviour
{
    private class Bone
    {
        public Transform transform;
        public GameObject gameObject;
        public Rigidbody2D rigidbody;
        public Collider2D boneCollider;
        public SpriteRenderer boneRenderer;

        public Vector3 startLocalPosition;
        public Vector3 reformStartPosition;
        public Quaternion reformStartRotation;

        public Bone(Transform transform, GameObject gameObject,
                Rigidbody2D rigidbody, Collider2D boneCollider,
                SpriteRenderer boneRenderer)
        {
            this.transform = transform;
            this.gameObject =  gameObject;
            this.rigidbody = rigidbody;
            this.boneCollider =  boneCollider;
            this.boneRenderer = boneRenderer;

            startLocalPosition = transform.localPosition;
        }
    }


    public KeyCode explodeInput;
    public Vector2 explosionForce;
    [Tooltip("distance in units to disable bone gameobjects")]
    public float deactivateRadius;
    [Tooltip("distance from player to spawn bone gameObjects at before animating reform")]
    public float reformRadius;
    public float reformTime;
    public bool canExplode;
    public bool exploded;
    List<Bone> bones;
    float reformCounter;

    [HideInInspector] public UnityEvent onExplode;
    [HideInInspector] public UnityEvent onReform;
    void Start()
    {
        bones = new List<Bone>();
        foreach (Transform child in transform)
        {
            bones.Add(
                    new Bone(child, child.gameObject,
                    child.GetComponent<Rigidbody2D>(),
                    child.GetComponent<Collider2D>(),
                    child.GetComponent<SpriteRenderer>()));
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(explodeInput))
        {
            if(!exploded && canExplode)
            {
                Explode();
                return;
            }

            if(exploded)
                Reform();
        }

        foreach (Bone bone in bones)
        {
            if(!bone.gameObject.activeInHierarchy)
                continue;
            
            if(Vector3.Distance(Vector3.zero, bone.transform.localPosition) > deactivateRadius)
                bone.gameObject.SetActive(false);

        }
    }

    private void Explode()
    {
        foreach (Bone bone in bones)
        {
            bone.gameObject.SetActive(true);
            bone.boneCollider.enabled = true;
            bone.rigidbody.isKinematic = false;
            bone.rigidbody.AddForceAtPosition( explosionForce,
                    (Vector2)transform.parent.position + Vector2.down,
                    ForceMode2D.Impulse);
        }
        exploded = true;
        onExplode.Invoke();
    }

    private void Reform()
    {
        StartCoroutine(ReformRoutine());
    }

    IEnumerator ReformRoutine()
    {
        foreach (Bone bone in bones)
        {
            // if disabled, it means it fell off
            if(!bone.gameObject.activeInHierarchy)
            {
                bone.gameObject.SetActive(true);
                bone.transform.position = 
                        (bone.transform.position - transform.position).normalized
                                * reformRadius + transform.position;
            }

            bone.rigidbody.isKinematic = true;
            bone.boneCollider.enabled = false;
            bone.reformStartPosition = transform.localPosition;
            bone.reformStartRotation = transform.localRotation;
        }

        while(reformCounter < reformTime)
        {
            reformCounter += Time.deltaTime;

            foreach (Bone bone in bones)
            {
                bone.transform.localPosition =
                        Vector3.Lerp(bone.reformStartPosition,
                                bone.startLocalPosition,
                                reformCounter / reformTime);

                bone.transform.localRotation =
                        Quaternion.Lerp(bone.reformStartRotation,
                                Quaternion.identity,
                                reformCounter / reformTime);
            }

            yield return null;
        }

        foreach (Bone bone in bones)
            bone.gameObject.SetActive(false);

        reformCounter = 0;
        exploded = false;
        onReform.Invoke();        
    }
}
