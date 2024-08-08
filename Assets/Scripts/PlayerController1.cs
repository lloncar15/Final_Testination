using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController1 : MonoBehaviour
{
    [SerializeField]
    public Sprite[] animationSprites;

    [SerializeField]
    public float speed;
    public bool laserActive;
    public System.Action killed;
    public GameObject paper;
    public Projectile laserPrefab;

    public float animationTime = 1.0f;
    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    [SerializeField]
    public GameObject paperLeftBoundary;
    [SerializeField]
    public GameObject paperRightBoundary;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;
        if (_animationFrame >= this.animationSprites.Length)
        {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    void Update()
    {
        if (GameController.Instance.isRunning == false)
        {
            return;
        }

        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += speed * Time.deltaTime;
        }


        // Clamp the position of the character so they do not go out of bounds
        float playerHalfScale = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        position.x = Mathf.Clamp(position.x, paperLeftBoundary.transform.position.x + playerHalfScale, paperRightBoundary.transform.position.x - playerHalfScale);
        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Only one laser can be active at a given time so first check that
        // there is not already an active laser
        if (!laserActive)
        {
            laserActive = true;

            Projectile laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.destroyed += OnLaserDestroyed;
        }
    }

    private void OnLaserDestroyed(Projectile laser)
    {
        laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Alien"))
        {
            this.gameObject.SetActive(false);
            GameController.Instance.loseCondition();
        }
    }

    public void Reset()
    {
        this.gameObject.SetActive(true);
    }
}
