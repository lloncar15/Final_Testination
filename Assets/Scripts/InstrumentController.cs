using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        if (SimonGameController.Instance.isRunning == false)
        {
            return;
        }
        if (SimonGameController.Instance.touchesEnabled == false)
        {
            return;
        }

        int number = 0;

        if (this.gameObject.name == "Instrument1")
        {
            number = 1;
        }
        else if (this.gameObject.name == "Instrument2")
        {
            number = 2;
        }
        else if (this.gameObject.name == "Instrument3")
        {
            number = 3;
        }
        else if (this.gameObject.name == "Instrument4")
        {
            number = 4;
        }
        else if (this.gameObject.name == "Instrument5")
        {
            number = 5;
        }

        SimonGameController.Instance.checkCorrect(number);
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx-click");
    }



}
