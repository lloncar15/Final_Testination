using UnityEngine;
using TMPro;

public class SnakeMenuController : MonoBehaviour
{
    public TMP_Text text;
    public bool canPressSpaceToContinue = false;
    public PlayerController2 snake;
    public SnakeGameController controller;

    public void Update()
    {
        if (canPressSpaceToContinue && Input.GetKeyDown(KeyCode.Space))
        {
            StopRestartText();
            controller.Reset();
            snake.ResetState();
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
