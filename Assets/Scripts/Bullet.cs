using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{ 
    private Player owner;
    private float strength;

    [SerializeField] private Sprite[] sprites;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    bool fire = false;
    Vector2 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        float moveAngle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, moveAngle - 90);

        if (fire)
        {
            rb.linearVelocity = velocity;
            fire = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && player != owner)
        {
            player.Damage(strength);
        }

        Destroy(this.gameObject);
    }

    public void SetOwner(Player player)
    {
        owner = player;
    }

    public void Fire(float strength, float speed, float direction, bool projectileGravity)
    {
        // if (projectileGravity == false)
        // {
        //     rb.gravityScale = 0;
        // }

        this.strength = strength;

        float angleInRadians = direction * Mathf.Deg2Rad;
        
        velocity = new Vector2(Mathf.Cos(angleInRadians) * speed, Mathf.Sin(angleInRadians) * speed);

        fire = true;
    }
}