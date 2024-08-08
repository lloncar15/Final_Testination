using UnityEngine;

public class Number : MonoBehaviour
{
    [SerializeField]
    public Sprite[] animationSprites;

    public float animationTime = 1.0f;

    public System.Action killed;
    public int number;

    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    public int row;
    public int column;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;
        
        if (_animationFrame >= this.animationSprites.Length) {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            this.gameObject.SetActive(false);
            bool correct = GameController.Instance.checkCorrectNumber(this.number);
            AliensGridController.Instance.NumberKilled(correct);
        }
    }
}
