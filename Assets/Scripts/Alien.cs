using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField]
    public Sprite[] animationSprites;

    public float animationTime = 1.0f;

    public System.Action killed;

    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet")) {
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
