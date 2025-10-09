using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private Intervals[] inter; //total objects that will be triggered

    // Update is called once per frame
    private void Update()
    {
        //time elapsed in intervals in beat
        foreach(Intervals interval in inter)
        {
            float sampledTime = (bgm.timeSamples / (bgm.clip.frequency * interval.GetIntervalLength(bpm)));
            interval.CheckNewIntervals(sampledTime);
            
        }
    }
}
[System.Serializable]
public class Intervals
{
    [SerializeField] private float steps; //1 = whole beats, 0.5 = every 2 beats
    //event that will be triggered in the beat
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;

    //get the beat length
    public float GetIntervalLength(float bpm1)
    {
        //Debug.Log("triggered");
        return 60f / (bpm1 *steps);
    }

    //check if cross a new beat or not
    public void CheckNewIntervals(float interval)
    {
        
        if(Mathf.FloorToInt(interval)!=lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            //Debug.Log("triggered");
            trigger.Invoke();
        }
    }
}
