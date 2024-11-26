using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject[] canons;
    public GameObject gameover;
    public Animator lose;
    // Start is called before the first frame update
    void Start()
    {
        gameover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag == "player")
        {
            gameover.SetActive(true);
        }
    }
    public void shoot()
    {

    }
}
