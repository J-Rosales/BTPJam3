using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelControl : MonoBehaviour
{
    public Vector2 boundariesMin;
    public Vector2 boundariesMax;
    Rect levelBoundaries;
    Transform player;
    [HideInInspector] public UnityEvent onPlayerOutOfBounds = new UnityEvent();
    bool boundaryLock;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if(!boundaryLock && player.position.x < boundariesMin.x
                || player.position.y < boundariesMin.y    
                || player.position.x > boundariesMax.x   
                || player.position.y > boundariesMax.y)
        {
            boundaryLock = true;
            onPlayerOutOfBounds.Invoke();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 bottomLeft = new Vector3(boundariesMin.x, boundariesMin.y, 0);
        Vector3 topLeft = new Vector3(boundariesMin.x, boundariesMax.y, 0);
        Vector3 topRight = new Vector3(boundariesMax.x, boundariesMax.y, 0);
        Vector3 bottomRight = new Vector3(boundariesMax.x, boundariesMin.y, 0);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }
}
