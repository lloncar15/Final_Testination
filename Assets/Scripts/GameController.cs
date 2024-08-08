using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class TupleList<T1, T2> : List<Tuple<T1, T2>>
{
    public void Add( T1 item, T2 item2 )
    {
        Add( new Tuple<T1, T2>( item, item2 ) );
    }
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    // CONSTANTS - SPACE INVADERS
    public TupleList<string, int> spaceInvaderEquations;
    private string currentEquation;
    private int currentSolution;

    [SerializeField]
    public GameObject[] possibleNumberParts;
    [SerializeField]
    public GameObject[] possibleOperators;

    [SerializeField]
    public GameObject leftPart;
    [SerializeField]
    public GameObject operatorPart;
    [SerializeField]
    public GameObject rightPart;

    public GameObject gradeA;
    public GameObject gradeF;

    public GameObject aliens;
    public GameObject gameOver;

    private System.Random randomGenerator;

    public AlienMenuController menu;

    public PlayerController1 playerController;

    public LevelLoader loader;

    public bool isRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            randomGenerator = new System.Random();
            fillSpaceInvaderEquations();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateEquation(int randomNumber) {
        int randomIndex = randomGenerator.Next(0, spaceInvaderEquations.Count);
        for (int i=randomIndex;i<spaceInvaderEquations.Count;i++) {
            if (randomNumber == spaceInvaderEquations[i].Item2) {
                currentEquation = spaceInvaderEquations[i].Item1;
                currentSolution = spaceInvaderEquations[i].Item2;

                int firstNumber = currentEquation[0] - '0';
                char operatorsPart = currentEquation[1];
                int secondNumber = currentEquation[2] - '0';

                leftPart.GetComponent<SpriteRenderer>().sprite = possibleNumberParts[firstNumber].GetComponent<SpriteRenderer>().sprite;
                rightPart.GetComponent<SpriteRenderer>().sprite = possibleNumberParts[secondNumber].GetComponent<SpriteRenderer>().sprite;
                if (operatorsPart == '+')
                {
                    operatorPart.GetComponent<SpriteRenderer>().sprite = possibleOperators[0].GetComponent<SpriteRenderer>().sprite;
                }
                else if (operatorsPart == '-')
                {
                    operatorPart.GetComponent<SpriteRenderer>().sprite = possibleOperators[1].GetComponent<SpriteRenderer>().sprite;
                }
                else if (operatorsPart == '*')
                {
                    operatorPart.GetComponent<SpriteRenderer>().sprite = possibleOperators[2].GetComponent<SpriteRenderer>().sprite;
                }
                else if (operatorsPart == '/')
                {
                    operatorPart.GetComponent<SpriteRenderer>().sprite = possibleOperators[3].GetComponent<SpriteRenderer>().sprite;
                }
                return;
            }
        }
        for (int i=0;i<randomIndex;i++)
        {
            if (randomNumber == spaceInvaderEquations[i].Item2)
            {
                currentEquation = spaceInvaderEquations[i].Item1;
                currentSolution = spaceInvaderEquations[i].Item2;

                int firstNumber = currentEquation[0] - '0';
                char operatorsPart = currentEquation[1];
                int secondNumber = currentEquation[2] - '0';

                leftPart.GetComponent<SpriteRenderer>().sprite = possibleNumberParts[firstNumber].GetComponent<SpriteRenderer>().sprite;
                rightPart.GetComponent<SpriteRenderer>().sprite = possibleNumberParts[secondNumber].GetComponent<SpriteRenderer>().sprite;
                if (operatorsPart == '+')
                {
                    operatorPart.GetComponent<SpriteRenderer>().sprite = possibleOperators[0].GetComponent<SpriteRenderer>().sprite;
                }
                else if (operatorsPart == '-')
                {
                    operatorPart.GetComponent<SpriteRenderer>().sprite = possibleOperators[1].GetComponent<SpriteRenderer>().sprite;
                }
                else if (operatorsPart == '*')
                {
                    operatorPart.GetComponent<SpriteRenderer>().sprite = possibleOperators[2].GetComponent<SpriteRenderer>().sprite;
                }
                else if (operatorsPart == '/')
                {
                    operatorPart.GetComponent<SpriteRenderer>().sprite = possibleOperators[3].GetComponent<SpriteRenderer>().sprite;
                }
                return;
            }
        }
    }

    public void winCondition()
    {
        isRunning = false;
        gradeA.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game2/Voice/Tips/vo-tips-win-A");
        AliensGridController.Instance.CancelInvoke();
        loader.LoadNextLevel();
    }

    public void loseCondition()
    {
        aliens.SetActive(false);
        gameOver.SetActive(true);
        isRunning = false;
        gradeF.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx-death");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game2/Voice/Tips/vo-tips-fail-F");

        menu.ShowRestartText();
    }

    public bool checkCorrectNumber(int number)
    {
        return number == this.currentSolution;
    }

    public void Reset()
    {
        this.isRunning = true;
        AliensGridController.Instance.Reset();
        playerController.Reset();
        aliens.SetActive(true);
        gameOver.SetActive(false);
        gradeA.SetActive(false);
        gradeF.SetActive(false);
    }

    private void fillSpaceInvaderEquations()
    {
        spaceInvaderEquations = new TupleList<string, int>()
        {
            {"0+0",0},
            {"0-0",0},
            {"0*0",0},
            {"0+1",1},
            {"0*1",0},
            {"0+2",2},
            {"0*2",0},
            {"0+3",3},
            {"0*3",0},
            {"0+4",4},
            {"0*4",0},
            {"0+5",5},
            {"0*5",0},
            {"0+6",6},
            {"0*6",0},
            {"0+7",7},
            {"0*7",0},
            {"0+8",8},
            {"0*8",0},
            {"0+9",9},
            {"0*9",0},
            {"1+0",1},
            {"1-0",1},
            {"1*0",0},
            {"1+1",2},
            {"1-1",0},
            {"1*1",1},
            {"1/1",1},
            {"1+2",3},
            {"1*2",2},
            {"1+3",4},
            {"1*3",3},
            {"1+4",5},
            {"1*4",4},
            {"1+5",6},
            {"1*5",5},
            {"1+6",7},
            {"1*6",6},
            {"1+7",8},
            {"1*7",7},
            {"1+8",9},
            {"1*8",8},
            {"1*9",9},
            {"2+0",2},
            {"2-0",2},
            {"2*0",0},
            {"2+1",3},
            {"2-1",1},
            {"2*1",2},
            {"2/1",2},
            {"2+2",4},
            {"2-2",0},
            {"2*2",4},
            {"2/2",1},
            {"2+3",5},
            {"2*3",6},
            {"2+4",6},
            {"2*4",8},
            {"2+5",7},
            {"2+6",8},
            {"2+7",9},
            {"3+0",3},
            {"3-0",3},
            {"3*0",0},
            {"3+1",4},
            {"3-1",2},
            {"3*1",3},
            {"3/1",3},
            {"3+2",5},
            {"3-2",1},
            {"3*2",6},
            {"3+3",6},
            {"3-3",0},
            {"3*3",9},
            {"3/3",1},
            {"3+4",7},
            {"3+5",8},
            {"3+6",9},
            {"4+0",4},
            {"4-0",4},
            {"4*0",0},
            {"4+1",5},
            {"4-1",3},
            {"4*1",4},
            {"4/1",4},
            {"4+2",6},
            {"4-2",2},
            {"4*2",8},
            {"4/2",2},
            {"4+3",7},
            {"4-3",1},
            {"4+4",8},
            {"4-4",0},
            {"4/4",1},
            {"4+5",9},
            {"5+0",5},
            {"5-0",5},
            {"5*0",0},
            {"5+1",6},
            {"5-1",4},
            {"5*1",5},
            {"5/1",5},
            {"5+2",7},
            {"5-2",3},
            {"5+3",8},
            {"5-3",2},
            {"5+4",9},
            {"5-4",1},
            {"5-5",0},
            {"5/5",1},
            {"6+0",6},
            {"6-0",6},
            {"6*0",0},
            {"6+1",7},
            {"6-1",5},
            {"6*1",6},
            {"6/1",6},
            {"6+2",8},
            {"6-2",4},
            {"6/2",3},
            {"6+3",9},
            {"6-3",3},
            {"6/3",2},
            {"6-4",2},
            {"6-5",1},
            {"6-6",0},
            {"6/6",1},
            {"7+0",7},
            {"7-0",7},
            {"7*0",0},
            {"7+1",8},
            {"7-1",6},
            {"7*1",7},
            {"7/1",7},
            {"7+2",9},
            {"7-2",5},
            {"7-3",4},
            {"7-4",3},
            {"7-5",2},
            {"7-6",1},
            {"7-7",0},
            {"7/7",1},
            {"8+0",8},
            {"8-0",8},
            {"8*0",0},
            {"8+1",9},
            {"8-1",7},
            {"8*1",8},
            {"8/1",8},
            {"8-2",6},
            {"8/2",4},
            {"8-3",5},
            {"8-4",4},
            {"8/4",2},
            {"8-5",3},
            {"8-6",2},
            {"8-7",1},
            {"8-8",0},
            {"8/8",1},
            {"9+0",9},
            {"9-0",9},
            {"9*0",0},
            {"9-1",8},
            {"9*1",9},
            {"9/1",9},
            {"9-2",7},
            {"9-3",6},
            {"9/3",3},
            {"9-4",5},
            {"9-5",4},
            {"9-6",3},
            {"9-7",2},
            {"9-8",1},
            {"9-9",0},
            {"9/9",1}
        };
    }
}
