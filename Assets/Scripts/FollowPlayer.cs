using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    [SerializeField] private float followSpeed;
    [SerializeField] private Transform player;
    Vector3 newPosition;
    
    LevelControl level;
    Camera gameCamera;
    Vector2 cameraSize;

    void Awake() 
    {
        gameCamera = GetComponent<Camera>();
        float height = 2f * gameCamera.orthographicSize;
        float width = height * gameCamera.aspect;
        cameraSize = new Vector2(width, height);
    }

    void Start()
    {
        level = FindObjectOfType<LevelControl>();
        if(player == null)
            player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player>().transform;
            Debug.Log("player not found: reassigning");
        }

        newPosition = new Vector3(
                Mathf.Clamp(player.transform.position.x, level.boundariesMin.x + cameraSize.x / 2 , level.boundariesMax.x - cameraSize.x / 2 ),
                Mathf.Clamp(player.transform.position.y, level.boundariesMin.y + cameraSize.y / 2,  level.boundariesMax.y - cameraSize.y / 2 ), 0);
        
        newPosition = Vector3.MoveTowards(transform.position, newPosition, followSpeed);
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
