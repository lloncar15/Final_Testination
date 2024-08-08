using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD;
using FMODUnity;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void Awake()
    {
        
    }

    public void LoadNextLevel()
    {
        int undex = SceneManager.GetActiveScene().buildIndex + 1;
        if (undex >= SceneManager.sceneCountInBuildSettings)
        {
            Application.Quit();
        }
        else
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        } 
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        FMODUnity.RuntimeManager.GetBus("Bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);

        SceneManager.LoadScene(levelIndex);
    }
}
