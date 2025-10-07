using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start1 : MonoBehaviour
{
    //public AudioSource loop;
    public AudioSource startmsc;
    public Animator cam;
    public Animator menu;
    public Animator logo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startMsc()
    {
        startmsc.Play();
    }
    public void mscLoop()
    {
        menu.SetTrigger("in");
        logo.SetTrigger("beat");
        cam.SetTrigger("beat");
        //loop.Play();
    }
}
