using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using FMOD;
using FMODUnity;

public class SnakeGameController : MonoBehaviour
{
    public Dictionary<string, int> charToInt = new Dictionary<string, int>()
    {
        {"A", 0},
        {"B", 1},
        {"C", 2},
        {"D", 3},
        {"E", 4},
        {"F", 5},
        {"G", 6},
        {"H", 7},
        {"I", 8},
        {"J", 9},
        {"K", 10},
        {"L", 11},
        {"M", 12},
        {"N", 13},
        {"O", 14},
        {"P", 15},
        {"R", 16},
        {"S", 17},
        {"T", 18},
        {"U", 19},
        {"V", 20},
        {"Z", 21},
        {"X", 22},
        {"Y", 23},
        {"Q", 24},
        {"W", 25}
    };


    public Collider2D gridArea;
    public string[] words;
    public FoodLetter[] letters;
    public Transform snakeHead;
    public int numberOfWords;
    public FoodLetter foodPrefab;
    public int score;
    public Sprite[] letterSprites;
    public SnakeMenuController menu;
    public LevelLoader loader;
    public bool isActive = false;
    bool isGameOver = false;

    public GameObject gradeA;
    public GameObject gradeF;

    public GameObject gameOver;

    public PlayerController2 p2;

    private string[] availableWords = new string[]
        {
            "KRIJES", "CLUSTER", "PASTOR", "AMOEBA", "ADVISE", "PIJAT", "BOOBS", "BEND", "PREY", "COME", "GOSLING"
        };

    public EventReference[] wordSounds;
    public EventReference nextWordSound;
    public EventReference firstWordSound;
    public EventReference helperSound;
    public EventReference failSound;


    private string possibleLetters = "ABCDEFGHIJKLMNOPRSTUVZXYQW";
    private int currentWordIndex = 0;
    private int currentCharIndex = 0;
    private int activeLetters = 4;
    private float distanceFromHead = 1.0f;

    public void Awake()
    {
        words = new string[numberOfWords];
        letters = new FoodLetter[activeLetters];
        GetRandomWordsFromPool();
    }

    public void Start()
    {
        SpawnNewLetters();
        StartCoroutine(firstWOrd());
        
    }

    IEnumerator firstWOrd()
    {
        yield return new WaitForSeconds(1);
        RuntimeManager.PlayOneShot(firstWordSound, transform.position);
        StartCoroutine(GameStartAfter());
        StartCoroutine(PlayWordAfterSound(1.24f, words[currentWordIndex]));
    }

    IEnumerator GameStartAfter()
    {
        yield return new WaitForSeconds(1);
        isActive = true;
    }

    public void Update()
    {
        if (!menu.canPressSpaceToContinue && !isGameOver && Input.GetKeyDown(KeyCode.Space) && isActive)
        {
            RuntimeManager.PlayOneShot(helperSound, transform.position);
            StartCoroutine(PlayWordAfterSound(2.5f, words[currentWordIndex]));
        }
    }

    public void GetRandomWordsFromPool()
    {
        List<string> availableWords2 = new List<string>();

        foreach (string word in availableWords)
        {
            availableWords2.Add(word);
        }

        for(int i = 0; i < numberOfWords; ++i)
        {
            int index = UnityEngine.Random.Range(0, availableWords2.Count);
            words[i] = availableWords2[index];
            availableWords2.RemoveAt(index);
        }
    }

    public void SpawnNewLetters()
    {
        if (currentWordIndex < words.Length)
        {
            string[] chosenLetters = new string[activeLetters];
            Vector3[] chosenPositions = GetPossiblePositions();

            // get the next letter of the next word
            string nextChar = words[currentWordIndex][currentCharIndex].ToString();
            chosenLetters[0] = nextChar;

            for (int i = 1; i < activeLetters; i++)
            {
                string chosen = possibleLetters[UnityEngine.Random.Range(0, possibleLetters.Length)].ToString();
                chosenLetters[i] = chosen;
            }


            // spawn all the letters
            for (int i = 0; i < activeLetters; i++)
            {
                SpawnLetter(i, chosenLetters[i], chosenPositions[i]);
            }
        }
    }

    public Vector3[] GetPossiblePositions()
    {
        Vector3[] chosenTransforms = new Vector3[activeLetters];

        List<Vector3> alreadyChosen = new List<Vector3>();
        for (int i = 0; i < activeLetters; i++)
        {
            Vector3 chosen = GetPossiblePosition(alreadyChosen);
            alreadyChosen.Add(chosen);
            chosenTransforms[i] = chosen;
        }

        return chosenTransforms;
    }

    public Vector3 GetPossiblePosition(List<Vector3> alreadyChosenPositions)
    {
        float snakePosX = snakeHead.transform.position.x;
        float snakePosY = snakeHead.transform.position.y;

        Vector3 pos = new Vector3(snakePosX, snakePosY, -2);

        Bounds bounds = gridArea.bounds;

        // Pick a random position inside the bounds
        while (Math.Abs(pos.x - snakePosX) < distanceFromHead)
        {
            pos.x = ReturnFloatWithRoundedDecimal(UnityEngine.Random.Range(bounds.min.x, bounds.max.x));
        }

        while (Math.Abs(pos.y - snakePosY) < distanceFromHead || alreadyChosenPositions.Contains(pos))
        {
            pos.y = ReturnFloatWithRoundedDecimal(UnityEngine.Random.Range(bounds.min.y, bounds.max.y));
        }

        return pos;
    }

    public float ReturnFloatWithRoundedDecimal(float num)
    {
        double toReturn = Math.Round(num);
        double decimalPart = num - toReturn;
        if (decimalPart < 0.5f)
        {
            decimalPart = 0;
        }
        else
        {
            decimalPart = 0.5f;
        }

        toReturn += decimalPart;
        return (float)toReturn;
    }

    public void SpawnLetter(int index, string letter, Vector3 location)
    {
        // spawn the letter on the grid not adjacent 2 spaces from the snake head
        FoodLetter foodLetter = Instantiate(foodPrefab, location, Quaternion.identity);
        foodLetter.letter = letter;
        foodLetter.setSprite(charToInt[letter], this);
        // put spawned object into letters
        letters[index] = foodLetter;
    }

    public bool IsRightLetter(string letter)
    {
        return words[currentWordIndex][currentCharIndex].ToString() == letter;
    }

    public void LetterCollected()
    {
        ++currentCharIndex;

        if (currentCharIndex == words[currentWordIndex].Length)
        {
            CompleteWord();
        }
    }

    public void CompleteWord()
    {
        ++currentWordIndex;

        

        // finish the game if the last word is completed
        if (currentWordIndex == words.Length)
        {
            StartCoroutine(StartWinGame());
            return;
        }

        RuntimeManager.PlayOneShot(nextWordSound, transform.position);
        StartCoroutine(PlayWordAfterSound(1.2f, words[currentWordIndex]));

        // clear the array
        currentCharIndex = 0;
        DestroyCurrentLetters();
        SpawnNewLetters();
    }

    public void GameOver()
    {
        isActive = false;
        gradeF.SetActive(true);
        gameOver.SetActive(true);
        menu.ShowRestartText();
        isGameOver = true;
        RuntimeManager.PlayOneShot(failSound, transform.position);
    }

    public void DestroyCurrentLetters()
    {
        foreach(FoodLetter go in letters)
        {
            Destroy(go.gameObject);
        }
    }

    public void Reset()
    {
        DestroyCurrentLetters();

        words = new string[numberOfWords];
        letters = new FoodLetter[activeLetters];
        currentWordIndex = 0;
        currentCharIndex = 0;
        gradeA.SetActive(false);
        gradeF.SetActive(false);
        gameOver.SetActive(false);
        GetRandomWordsFromPool();
        SpawnNewLetters();
        isActive = true;
        isGameOver = false;
    }

    IEnumerator StartWinGame()
    {
        isActive = false;
        p2.isGameOver = true;
        gradeA.SetActive(true);

        yield return new WaitForSeconds(1);

        loader.LoadNextLevel();
    }

    public IEnumerator PlayWordAfterSound(float waitTime, string wordString)
    {
        yield return new WaitForSeconds(waitTime);
        int index = Array.IndexOf(availableWords, wordString);
        RuntimeManager.PlayOneShot(wordSounds[index], transform.position);
    }
}
