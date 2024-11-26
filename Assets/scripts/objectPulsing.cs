using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPulsing : MonoBehaviour
{
    [SerializeField] private Vector3 startSize;
    [SerializeField] private float pulseSize = 1.15f;
    [SerializeField] private Animator pulseAnim;
    //bool pulsing = false;
    // Start is called before the first frame update
    void Start()
    {
        startSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        /*        if(pulsing)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * 5f);
                }
                if(transform.localScale==startSize)
                {
                    pulsing = false;
                }*/
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * 5f);
    }
    public void pulse()
    {
        //pulsing = true;
        transform.localScale = startSize * pulseSize;
    }
    public void pulseAnimation()
    {
        pulseAnim.SetTrigger("beat");
    }
}
