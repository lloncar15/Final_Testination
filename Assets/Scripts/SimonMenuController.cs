using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimonMenuController : MonoBehaviour
{
    public TMP_Text text;
    bool canPressSpaceToContinue = false;

    public void Update()
    {
        if (canPressSpaceToContinue && Input.GetKeyDown(KeyCode.Space))
        {
            StopRestartText();
            SimonGameController.Instance.Reset();
        }
    }

    public void ShowRestartText()
    {
        canPressSpaceToContinue = true;
        //TODO: do anmation and start
        text.gameObject.SetActive(true);
    }

    public void StopRestartText()
    {
        canPressSpaceToContinue = false;
        //TODO: do animation and sound
        text.gameObject.SetActive(false);
    }
}
