using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using FMODUnity;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2 : MonoBehaviour
{

    private List<Transform> segments = new List<Transform>();
    public Transform segmentPrefab;
    public Vector2 direction = new Vector2(-0.5f, 0f);
    private Vector2 input;
    public int initialSize = 5;
    public SnakeGameController controller;

    public EventReference collectEvent;

    public LetterSprite letterSprite;

    private int dummyCounter = 0;


    public Sprite[] animationSprites;
    private SpriteRenderer _spriteRenderer;
    int animationSpriteIndex = 0;
    int animationCounter = 0;

    public bool isGameOver = false;

    private void AnimateSprite()
    {
        ++animationCounter;
        if (animationCounter == 2)
        {
            if (animationSpriteIndex == 0)
            {
                animationSpriteIndex = 1;
                _spriteRenderer.sprite = animationSprites[1];
            }
            else
            {
                animationSpriteIndex = 0;
                _spriteRenderer.sprite = animationSprites[0];
            }
            animationCounter = 0;
        }
    }

    private void Awake()
    {
        ResetState();
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
            // Only allow turning up or down while moving in the x-axis
            if (direction.x != 0f)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    input = new Vector2(0f, 0.5f);

                    transform.rotation = Quaternion.Euler(0, 0, 270);

                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y) * -1, transform.localScale.z);
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    input = new Vector2(0f, -0.5f);

                    transform.rotation = Quaternion.Euler(0, 0, 90);

                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
                }
            }
            // Only allow turning left or right while moving in the y-axis
            else if (direction.y != 0f)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    input = new Vector2(0.5f, 0f);

                    transform.rotation = Quaternion.Euler(0, 0, 0);

                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    input = new Vector2(-0.5f, 0f);

                    transform.rotation = Quaternion.Euler(0, 0, 0);

                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
                }
            } 
    }

    private void FixedUpdate()
    {
        ++dummyCounter;
        if (!isGameOver && dummyCounter >= 10 && controller.isActive)
        {
            dummyCounter = 0;
            // Set the new direction based on the input
            if (input != Vector2.zero)
            {
                direction = input;
            }

            // Set each segment's position to be the same as the one it follows. We
            // must do this in reverse order so the position is set to the previous
            // position, otherwise they will all be stacked on top of each other.
            for (int i = segments.Count - 1; i > 0; i--)
            {
                segments[i].position = segments[i - 1].position;
            }

            // Move the snake in the direction it is facing
            // Round the values to ensure it aligns to the grid
            float x = transform.position.x + direction.x;
            float y = transform.position.y + direction.y;

            transform.position = new Vector3(x, y, -2);
            AnimateSprite();
        }
    }

    public void Grow(string letter)
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);

        LetterSprite s = Instantiate(letterSprite, segment.transform);
        s.GetComponent<SpriteRenderer>().sprite = controller.letterSprites[controller.charToInt[letter]];
        s.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void ResetState()
    {
        direction = new Vector2(0.5f, 0f);
        transform.position = Vector3.zero;

        // Start at 1 to skip destroying the head
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);

        // -1 since the head is already in the list
        Grow("S");
        Grow("N");
        Grow("A");
        Grow("K");
        Grow("E");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            FoodLetter obj = other.gameObject.GetComponent<FoodLetter>();
            if (controller.IsRightLetter(obj.letter))
            {
                RuntimeManager.PlayOneShot(collectEvent, transform.position);

                Grow(obj.letter);
                controller.LetterCollected();
                controller.DestroyCurrentLetters();
                controller.SpawnNewLetters();
            }
            else
            {
                controller.GameOver();
            }
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            controller.GameOver();
        }
        else if (other.gameObject.CompareTag("Segment") && controller.isActive)
        {
            controller.GameOver();
        }
    }
}