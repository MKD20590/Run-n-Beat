using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskHeadphones : MonoBehaviour
{
    public GameObject[] mask;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<mask.Length;i++)
        {
            mask[i].GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
