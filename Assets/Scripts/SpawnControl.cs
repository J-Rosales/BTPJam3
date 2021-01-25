using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    public Transform[] spawns;

    Player player;

    private void Awake()
    {
        
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        FindObjectOfType<LevelControl>().onPlayerOutOfBounds.AddListener(Respawn);
    }

    // returns player to the right-most spawn to its left;
    void Respawn()
    {
        Debug.Log("respawning");
        Transform rightmost = spawns[0];
        foreach (Transform spawn in spawns)
        {
            if(spawn.transform.position.x > rightmost.position.x
                    && spawn.position.x < player.transform.position.x)
                rightmost = spawn;
        }
        player.transform.position = rightmost.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void Update()
    {
        
    }
}
