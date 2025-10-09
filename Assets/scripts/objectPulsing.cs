using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPulsing : MonoBehaviour
{
    [SerializeField] private Vector3 startSize;
    [SerializeField] private float pulseSize = 1.15f;
    [SerializeField] private Animator pulseAnim;
    // Start is called before the first frame update
    void Start()
    {
        startSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * 5f);
    }
    public void Pulse()
    {
        transform.localScale = startSize * pulseSize;
    }
    public void PulseAnimation()
    {
        pulseAnim.SetTrigger("beat");
    }
}
