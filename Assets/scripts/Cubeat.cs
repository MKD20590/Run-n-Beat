using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Cubeat : Boss
{
    [SerializeField] private CinemachineVirtualCamera cam1;
    [SerializeField] private Vector3 cam;

    [SerializeField] private Animator cubeat;
    [SerializeField] private Animator slide;
    [SerializeField] private Animator laser;
    [SerializeField] private Animator[] spike;

    [SerializeField] private GameManager gm;

    [SerializeField] private GameObject leftSpeakerUp;
    [SerializeField] private GameObject leftSpeakerDown;
    [SerializeField] private GameObject rightSpeakerUp;
    [SerializeField] private GameObject rightSpeakerDown;

    [SerializeField] private Vector3 startSizeLU;
    [SerializeField] private Vector3 startSizeLD;
    [SerializeField] private Vector3 startSizeRU;
    [SerializeField] private Vector3 startSizeRD;
    [SerializeField] private float pulseSize = 1.3f;

    public int count = 0;
    int count1 = 0;
    int count2 = -1;
    int count3 = 0;
    bool slideCount = false;

    int spikeAll;
    int spike1;
    int spike2;
    int spike3;
    int spike4;
    int spike5;

    bool countSpike1 = false;
    //int spikeTotal = 64;
    //bool left = true;
    // Start is called before the first frame update
    void Start()
    {
        cam = cam1.transform.position;

        spikeAll = Random.Range(0, 63);
        spike1 = Random.Range(0, 20);
        spike2 = Random.Range(21, 30);
        spike3 = Random.Range(31, 40);
        spike4 = Random.Range(41, 50);
        spike5 = Random.Range(51, 63);

        startSizeLU = leftSpeakerUp.transform.localScale;
        startSizeRU = rightSpeakerUp.transform.localScale;
        startSizeLD = leftSpeakerDown.transform.localScale;
        startSizeRD = rightSpeakerDown.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(count);
        cam1.transform.position = Vector3.Lerp(Camera.main.transform.position, cam, Time.deltaTime * 2f);

        rightSpeakerUp.transform.localScale = Vector3.Lerp(rightSpeakerUp.transform.localScale, startSizeRU, Time.deltaTime * 2f);
        leftSpeakerUp.transform.localScale = Vector3.Lerp(leftSpeakerUp.transform.localScale, startSizeLU, Time.deltaTime * 2f);
        rightSpeakerDown.transform.localScale = Vector3.Lerp(rightSpeakerDown.transform.localScale, startSizeRD, Time.deltaTime * 2f);
        leftSpeakerDown.transform.localScale = Vector3.Lerp(leftSpeakerDown.transform.localScale, startSizeLD, Time.deltaTime * 2f);
    }

    public void NormalPulsing()
    {
        if (count <= 14 && !gm.isDied)
        {
            if (count1 < 3)
            {
                rightSpeakerUp.transform.localScale = startSizeRU * pulseSize;
                leftSpeakerUp.transform.localScale = startSizeLU * pulseSize;
                rightSpeakerDown.transform.localScale = startSizeRD * pulseSize;
                leftSpeakerDown.transform.localScale = startSizeLD * pulseSize;
            }
            else if (count1 >= 4 && count1 <= 6)
            {
                rightSpeakerUp.transform.localScale = startSizeRU * pulseSize;
                leftSpeakerUp.transform.localScale = startSizeLU * pulseSize;
                rightSpeakerDown.transform.localScale = startSizeRD * pulseSize;
                leftSpeakerDown.transform.localScale = startSizeLD * pulseSize;
            }

            else if(count1>=8 && count1<=10)
            {
                rightSpeakerUp.transform.localScale = startSizeRU * pulseSize;
                leftSpeakerUp.transform.localScale = startSizeLU * pulseSize;
                rightSpeakerDown.transform.localScale = startSizeRD * pulseSize;
                leftSpeakerDown.transform.localScale = startSizeLD * pulseSize;
            }
            else if (count1 >= 11 && count1 <= 17)
            {
                rightSpeakerUp.transform.localScale = startSizeRU * pulseSize;
                leftSpeakerUp.transform.localScale = startSizeLU * pulseSize;
                rightSpeakerDown.transform.localScale = startSizeRD * pulseSize;
                leftSpeakerDown.transform.localScale = startSizeLD * pulseSize;
            }
            else if (count1 >= 19 && count1 <= 21)
            {
                rightSpeakerUp.transform.localScale = startSizeRU * pulseSize;
                leftSpeakerUp.transform.localScale = startSizeLU * pulseSize;
                rightSpeakerDown.transform.localScale = startSizeRD * pulseSize;
                leftSpeakerDown.transform.localScale = startSizeLD * pulseSize;
            }
            else if (count1 >= 23 && count1 <= 25)
            {
                rightSpeakerUp.transform.localScale = startSizeRU * pulseSize;
                leftSpeakerUp.transform.localScale = startSizeLU * pulseSize;
                rightSpeakerDown.transform.localScale = startSizeRD * pulseSize;
                leftSpeakerDown.transform.localScale = startSizeLD * pulseSize;
            }
            else if (count1 >= 27 && count1 <= 40)
            {
                rightSpeakerUp.transform.localScale = startSizeRU * pulseSize;
                leftSpeakerUp.transform.localScale = startSizeLU * pulseSize;
                rightSpeakerDown.transform.localScale = startSizeRD * pulseSize;
                leftSpeakerDown.transform.localScale = startSizeLD * pulseSize;
            }
            count1++;
        }

    }
    public void Pulsing()
    {
        count++;
        if(count>15 && !gm.isDied)
        {
            Camera.main.transform.position = new Vector3(cam.x, cam.y-0.1f, cam.z + 0.1f);

            rightSpeakerUp.transform.localScale = startSizeRU * pulseSize;
            leftSpeakerUp.transform.localScale = startSizeLU * pulseSize;
            rightSpeakerDown.transform.localScale = startSizeRD * pulseSize;
            leftSpeakerDown.transform.localScale = startSizeLD * pulseSize;
            //speaker.Play("beat");
        }

    }

    public void SpikeAttack()
    {
        if(count == 160)
        {
            cubeat.SetTrigger("defeat");
        }
        if (count > 30 && !gm.isDied && count <= 160)
        {
            if (!countSpike1)
            {
                countSpike1 = true;
                if(count<63)
                {
                    cubeat.SetTrigger("spike1");
                }
                spike[spikeAll].Play("in");

                if (count>=63)
                {
                    if(!slideCount)
                    {
                        slideCount = true;
                        cubeat.SetTrigger("slide");
                        slide.SetTrigger("slide");
                    }
                    cubeat.SetTrigger("spike2");
                    spike[spike1].Play("in");
                    spike[spike2].Play("in");
                    spike[spike3].Play("in");
                    spike[spike4].Play("in");
                    spike[spike5].Play("in");
                }
            }
            else
            {
                spike[spikeAll].Play("in_spike");

                if (count >= 63)
                {
                    spike[spike1].Play("in_spike");
                    spike[spike2].Play("in_spike");
                    spike[spike3].Play("in_spike");
                    spike[spike4].Play("in_spike");
                    spike[spike5].Play("in_spike");
                }
                spikeAll = Random.Range(0, 63);
                spike1 = Random.Range(0, 20);
                spike2 = Random.Range(21, 30);
                spike3 = Random.Range(31, 40);
                spike4 = Random.Range(41, 50);
                spike5 = Random.Range(51, 63);
                countSpike1 = false;
            }
        }
    }
    public void Lasers()
    {
        count2++;
        if (count2>=7 && !gm.isDied && count3<=5)
        {

            if (count2 <= 8)
            {

                if (laser.GetBool("in") == false)
                {
                    cubeat.SetTrigger("laser");
                    laser.SetBool("in", true);
                }
                else
                {
                    laser.SetBool("in", false);
                }
            }
            else if(count2 >= 9)
            {

                if (laser.GetBool("in1") == false)
                {
                    cubeat.SetTrigger("laser");
                    laser.SetBool("in1", true);
                }
                else
                {
                    laser.SetBool("in1", false);
                }
            }

        }
        if(count2>=10)
        {
            count3++;
            count2 = 6;
        }
    }
}
