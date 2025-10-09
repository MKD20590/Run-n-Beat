using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    //public AudioSource loop;
    public AudioSource startMusic;
    public Animator cam;
    public Animator menu;
    public Animator logo;
    public void StartMusic()
    {
        startMusic.Play();
    }
    public void MusicLoop()
    {
        menu.SetTrigger("in");
        logo.SetTrigger("beat");
        cam.SetTrigger("beat");
    }
}
