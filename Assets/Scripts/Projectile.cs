using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Vector3 direction = Vector3.up;
    public System.Action<Projectile> destroyed;
    public new BoxCollider2D collider { get; private set; }

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnDestroy()
    {
        if (destroyed != null)
        {
            destroyed.Invoke(this);
        }
    }

    private void Update()
    {
        if (GameController.Instance.isRunning == false)
        {
            Destroy(this.gameObject);
            return;
        }
        if (this.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            transform.position -= direction * speed * Time.deltaTime;
        }
    }

    private void CheckCollision(Collider2D other)
    {
        /*Bunker bunker = other.gameObject.GetComponent<Bunker>();

        if (bunker == null || bunker.CheckCollision(collider, transform.position))
        {
            Destroy(gameObject);
        }*/

        bool playerBullet = this.gameObject.layer == LayerMask.NameToLayer("Bullet") && other.gameObject.layer == LayerMask.NameToLayer("Alien");
        if (other.gameObject.layer == LayerMask.NameToLayer("BulletBoundary") || playerBullet)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }
}
