using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        var main = particle.main;
        main.startLifetime = 0.8f;
        //main.duration = 0.1f;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playParticle()
    {
        particle.Play();
    }
}
