//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHover : MonoBehaviour
{
    [SerializeField] private float targetChangeTime;
    [SerializeField] private float hoverSpeed;
    
    [Tooltip("Normalized multiplier of hover speed when easing")]
    [SerializeField] private float easeMultiplier;
    
    [Tooltip("Normalized start/end time for lower hover speed. I.e. 0.1f means starting/ending 0.1f of animation will be slower")]
    [SerializeField] private float easeTime;
    
    [Tooltip("z affects Ortographic Size.")]
    [SerializeField] private Vector3 changeMagnitude;

    Camera introCamera;
    Vector3 startPosition;
    Vector3 targetPosition;
    float startSize;
    float targetSize;
    float startHoverSpeed;
    float targetChangeCounter;

    void Awake()
    {
        introCamera = GetComponent<Camera>();    
    }

    void Start()
    {
        startPosition = transform.position;
        startSize = introCamera.orthographicSize;
        startHoverSpeed = hoverSpeed;
    }

    void Update()
    {
        targetChangeCounter+= Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, hoverSpeed);
        introCamera.orthographicSize = Mathf.MoveTowards(
                introCamera.orthographicSize, targetSize, hoverSpeed);
        if(targetChangeCounter > targetChangeTime)
        {

            // easing update
            if(targetChangeCounter < easeTime || targetChangeCounter > targetChangeTime - easeTime)
                hoverSpeed = startHoverSpeed * easeMultiplier;
            else
                hoverSpeed = startHoverSpeed;

            ChangeTargets();
            targetChangeCounter = 0;
        }
    }

    private void ChangeTargets()
    {
        targetPosition = startPosition + new Vector3(
                startPosition.x + Random.Range(-changeMagnitude.x, changeMagnitude.x),
                startPosition.y + Random.Range(-changeMagnitude.y, changeMagnitude.y),
                startPosition.z);
        targetSize = startSize + Random.Range(-changeMagnitude.z, changeMagnitude.z);
    }
}
