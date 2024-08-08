using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;
using System;

public class WaveformController : MonoBehaviour
{
    [SerializeField]
    public FMODUnity.StudioEventEmitter sound;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown() {
        if (!SimonGameController.Instance.touchesEnabled)
        {
            return;
        }
        if (SimonGameController.Instance.isRunning == false)
        {
            return;
        }

        int number = 0;
        float waitingTime = 0.0f;

        if (this.gameObject.name == "Waveform1")
        {
            number = 1;
            waitingTime = 6.5f;
        } else if (this.gameObject.name == "Waveform2")
        {
            number = 2;
            waitingTime = 14f;
        }
        else if (this.gameObject.name == "Waveform3")
        {
            number = 3;
            waitingTime = 16f;
        }
        else if (this.gameObject.name == "Waveform4")
        {
            number = 4;
            waitingTime = 21.5f;
        }
        else if (this.gameObject.name == "Waveform5")
        {
            number = 5;
            waitingTime = 30f;
        }

        SimonGameController.Instance.touchesEnabled = false;

        sound.SetParameter("KDNP", number);

        IEnumerator cor = WaitUntilEnd(waitingTime);
        StartCoroutine(cor);


        IEnumerator cor2 = enableTouch(waitingTime + 1);
        StartCoroutine(cor2);
    }

    private IEnumerator enableTouch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SimonGameController.Instance.touchesEnabled = true;
    }

    private IEnumerator WaitUntilEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        sound.SetParameter("KDNP", 0);
    }
}
