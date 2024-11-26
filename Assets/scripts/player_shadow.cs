using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_shadow : MonoBehaviour
{
    public GameObject player;
    Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move.x = player.transform.position.x;
        move.y = -0.165f;
        move.z = player.transform.position.z;
        transform.position = Vector3.MoveTowards(transform.position,move,20f);
    }
}
