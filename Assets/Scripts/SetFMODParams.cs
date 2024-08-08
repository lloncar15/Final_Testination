using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SetFMODParams : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    public EventReference EventReference;

    public bool isScary = false;
    public bool isChill = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(EventReference);
        instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        float scary = isScary ? 1.0f : 0.0f;
        instance.setParameterByName("Narration-Scare", scary);

        float chill = isChill ? 1.0f : 0.0f;
        instance.setParameterByName("Narration-Chill", chill);
    }
}
