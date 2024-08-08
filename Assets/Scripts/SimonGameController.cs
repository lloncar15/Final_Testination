using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonGameController : MonoBehaviour
{
    public static SimonGameController Instance;

    [SerializeField]
    public GameObject[] waveForms;

    private Dictionary<int, List<int>> soundsForLevels;
    public int currentLevel = 1;
    public int currentIndex = 0;

    public SimonMenuController menu;

    public GameObject gradeA;
    public GameObject gradeF;

    public GameObject waves;
    public GameObject instruments;
    public GameObject gameOver;

    public bool isRunning = false;
    public bool touchesEnabled = false;
    public bool hasWon = false;

    public LevelLoader loader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        touchesEnabled = true;
        currentLevel = 1;
        currentIndex = 0;
        isRunning = true;
        soundsForLevels = new Dictionary<int, List<int>>()
        {
            { 1,new List<int>{4} },
            { 2,new List<int>{ 4,2 } },
            { 3,new List<int>{ 4,2,1} },
            { 4,new List<int>{ 4,2,1,5} },
            { 5,new List<int>{ 4,2,1,5,3} },
        };
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Reset()
    {
        isRunning = true;
        currentLevel = 1;
        currentIndex = 0;

        gradeA.SetActive(false);
        waves.SetActive(true);
        instruments.SetActive(true);
        gradeF.SetActive(false);
        gameOver.SetActive(false);

        for (int i=1;i<5;i++)
        {
            waveForms[i].SetActive(false);
        }
    }

    public void checkCorrect(int pressed)
    {
        int needed = soundsForLevels[currentLevel][currentIndex];
        bool correct = needed == pressed;

        if (correct)
        {
            if (currentIndex == soundsForLevels[currentLevel].Count - 1)
            {
                currentLevel++;
                if (currentLevel > 5)
                {
                    isRunning = false;
                    hasWon = true;

                    gradeA.SetActive(true);
                    waves.SetActive(false);
                    instruments.SetActive(false);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Game2/Voice/Tips/vo-tips-win-A");
                    loader.LoadNextLevel();
                    return;
                }
                waveForms[currentLevel - 1].SetActive(true);
                currentIndex = 0;
            } else
            {
                currentIndex++;
            }
        }
        else
        {
            isRunning = false;
            menu.ShowRestartText();

            gradeF.SetActive(true);
            waves.SetActive(false);
            instruments.SetActive(false);
            gameOver.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot("event:/sfx-death");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game2/Voice/Tips/vo-tips-fail-F");

    }
    }

}
