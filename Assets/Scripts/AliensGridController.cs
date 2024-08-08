using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensGridController : MonoBehaviour
{
    public static AliensGridController Instance;

    [SerializeField]
    public Alien[] alienPrefabs;
    [SerializeField]
    public Number[] numberPrefabs;
    
    public int rows = 5;
    public int columns = 10;

    [SerializeField]
    public GameObject leftBoundary;
    [SerializeField]
    public GameObject rightBoundary;

    [SerializeField]
    public AnimationCurve speed;

    [SerializeField]
    public Projectile missilePrefab;
    public float missileSpawnRate = 1.0f;

    public List<int> possibleNumbers;
    private bool numbersReady = false;

    public int numberOfNumbers = 0;
    public int numberKilled;

    public int amountKilled { get; private set; }
    public int amountAlive => this.totalInvanders - this.amountKilled;
    public int totalInvanders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvanders;

    private System.Random randomNum = new System.Random();

    private Vector3 _direction = Vector2.right;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        possibleNumbers = new List<int>();
        amountKilled = 0;
        numberKilled = 0;
        numberOfNumbers = 0;

        CreateAliens();
    }

    public void Reset()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        possibleNumbers.Clear();
        numberKilled = 0;
        numberOfNumbers = 0;
        amountKilled = 0;
        CreateAliens();
        calculatePossibleNumbers();
    }

    public void CreateAliens()
    {
        for (int row = 0; row < this.rows; row++)
        {
            float width = 1.0f * (this.columns - 1);
            float height = -1.3f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2 - 1.2f, -height / 2 - 1.0f);

            for (int col = 0; col < this.columns; col++)
            {
                Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 1.0f), -2.0f);
                int isNumber = randomNum.Next(0, 3);
                if (isNumber == 1)
                {
                    int numberDec = randomNum.Next(0, 10);
                    Number number = Instantiate(this.numberPrefabs[numberDec], this.transform);
                    number.number = numberDec;
                    Vector3 position = rowPosition;
                    position.x += col * 1.2f;
                    number.transform.position = position;
                    number.row = row;
                    number.column = col;

                    numberOfNumbers++;
                }
                else
                {
                    Alien alien = Instantiate(this.alienPrefabs[row], this.transform);
                    alien.killed += InvaderKilled;
                    Vector3 position = rowPosition;
                    position.x += col * 1.2f;
                    alien.transform.position = position;
                }
            }
        }
    }

    private void calculatePossibleNumbers()
    {
        int[] possibleNums = new int[columns];
        int[] lastIndexes = new int[columns];
        for (int i = 0; i < columns; i++)
        {
            possibleNums[i] = -1;
            lastIndexes[i] = rows;
        }

        foreach (Transform tr in this.transform)
        {
            if (!tr.gameObject.activeSelf)
            {
                continue;
            }

            Number num = tr.GetComponent<Number>();
            if (num is not null)
            {
                int curNum = num.number;
                if (possibleNums[num.column] == -1 || lastIndexes[num.column] > num.row)
                {
                    possibleNums[num.column] = curNum;
                    lastIndexes[num.column] = num.row;
                }
            }

        }

        for (int i = 0; i < columns; i++)
        {
            if (possibleNums[i] != -1)
            {
                possibleNumbers.Add(possibleNums[i]);
            }
        }
        numbersReady = true;
    }

    private void Start()
    {
        calculatePossibleNumbers();
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    private void MissileAttack()
    {
        // No missiles should spawn when no invaders are alive
        if (amountAlive == 0)
        {
            return;
        }
        if (GameController.Instance.isRunning == false)
        {
            return;
        }

        foreach (Transform invader in transform)
        {
            // Any invaders that are killed cannot shoot missiles
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Random chance to spawn a missile based upon how many invaders are
            // alive (the more invaders alive the lower the chance)

            if (Random.value < (1f / (float)amountAlive))
            {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.isRunning == false)
        {
            return;
        }
        if (numbersReady)
        {
            int randNum = randomNum.Next(0, possibleNumbers.Count);
            GameController.Instance.updateEquation(possibleNumbers[randNum]);
            possibleNumbers.Clear();
            numbersReady = false;
        }

        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        float alienSize = alienPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float leftEdge = leftBoundary.transform.position.x + alienSize;
        float rightEdge = rightBoundary.transform.position.x - alienSize;

        foreach (Transform alien in this.transform)
        {
            if (!alien.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (_direction == Vector3.right && alien.position.x >= rightEdge)
            {
                AdvanceRow();
            }
            else if (_direction == Vector3.left && alien.position.x <= leftEdge)
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    public void NumberKilled(bool correct)
    {
        this.amountKilled++;
        this.numberKilled++;

        if (numberKilled >= numberOfNumbers)
        {
            GameController.Instance.winCondition();
            return;
        }

        if (correct)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game1/SFX/sfx-number-correct");
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game1/SFX/sfx-number-wrong");
            GameController.Instance.loseCondition();
            return;
        }

        // check if correct number
        calculatePossibleNumbers();
        if (numbersReady)
        {
            int randNum = randomNum.Next(0, possibleNumbers.Count);
            GameController.Instance.updateEquation(possibleNumbers[randNum]);
            possibleNumbers.Clear();
            numbersReady = false;
        }

    }

    private void InvaderKilled() 
    {
        this.amountKilled++;
        
    }
}
