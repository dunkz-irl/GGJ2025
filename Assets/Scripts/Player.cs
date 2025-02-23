using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public bool IsMainMenuPlayer = false;
    [SerializeField]
    public Vector2 MainMenuStartImpulse;

    [SerializeField]
    public string actionKey = "space";
    [SerializeField]
    private float spinRate = 1;

    private float spinDirection = 1f;
    [SerializeField]
    private float growRate = 1; 
    [SerializeField]
    private float floatRate = .1f; 
    [SerializeField]
    private float startingGravityScale = 0.001f;

    [Header("Projectile Variables")]
    [SerializeField]
    private Bullet projectilePrefab;
    [SerializeField]
    private float projectileSpeedCoeff = 5;
    [SerializeField]
    private AnimationCurve projectileChargeCurve = new AnimationCurve();
    [SerializeField]
    private float projectileStrength = 5;
    [SerializeField]
    private bool projectileGravity = false;

    [Header("Charge Dash Variables")]
    [SerializeField]
    private float impulseMagnitude = 5f;
    [SerializeField]
    private float chargeIncrement = 1f;
    [SerializeField]
    private float sizeChargeCoeff = 1f;
    [SerializeField]
    private AnimationCurve sizeChargeCurve = new AnimationCurve();
    [SerializeField]
    private float spinChargeCoeff = 50f;
    [SerializeField]
    private AnimationCurve spinChargeCurve = new AnimationCurve();
    [SerializeField]
    private float impulseChargeCoeff = 1f;
    [Space(8)]

    private float projectileCharge = 0f;
    private float spinRateCharge = 0f;
    private float impulseMagnitudeCharge = 0f;
    
    [SerializeField]
    private float invulnTimeAfterDamage = 1f;
    bool isDamaged = false;

    private bool isCharging = false;
    private float angle = 0;  
    private float size = 1;
    private float chargeTime = 0f;

    private ArrowDrawer arrowDrawer;
    private Rigidbody2D rb;

    private ContactFilter2D filter = new ContactFilter2D().NoFilter();
    private List<Collider2D> colliders = new List<Collider2D>();

    [SerializeField]
    Animation spriteAnimation;

    [SerializeField]
    private Color playerColor = Color.white;
    public Color GetPlayerColor { get => playerColor; }

    // Start is called once before the first execution of Update
    void Start()
    {
        arrowDrawer = GetComponent<ArrowDrawer>();
        rb = GetComponent<Rigidbody2D>();  

        rb.gravityScale = startingGravityScale;

        if (IsMainMenuPlayer)
        {
            rb.linearVelocity = MainMenuStartImpulse;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make control arrow spin
        angle += (spinRate * spinDirection + spinRateCharge) * Time.deltaTime;
        // make player grow, when not charging
        if (!isCharging)
        {
            if (CheckCollidingGeometry() < 2)
            {
                size += growRate * Time.deltaTime;
            }
        }
        // make player float
        rb.gravityScale -= floatRate * Time.deltaTime;

        if (IsMainMenuPlayer)
        {
            return;
        }

        // update size
        transform.localScale = Vector3.one * size;
        // Draw the arrow at the current angle and size
        float arrowDistance = transform.localScale.x/2 + 0.3f;
        arrowDrawer.DrawArrow(angle, arrowDistance);

        HandleInput();

        // Counter rotate child sprite renderer
    }

    private int CheckCollidingGeometry()
    {
        GetComponent<Collider2D>().Overlap(filter, colliders);

        int numCollidingGeometry = 0;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("StaticGeometry"))
            {
                numCollidingGeometry++;
                // Early out as we only care if it's less than 2
                if (numCollidingGeometry >= 2)
                {
                    return numCollidingGeometry;
                }
            }
        }

        return numCollidingGeometry;
    }

    private void HandleInput()
    {
        if (GameManager.Instance.currentGameState != GameManager.GameState.Running)
        {
            return;
        }

        // Hold
        if (Input.GetKeyDown(actionKey))
        {
            isCharging = true;
        }
        if (Input.GetKey(actionKey))
        {
            if (isCharging)
            {
                chargeTime += Time.deltaTime;

                size -= chargeIncrement * Time.deltaTime * sizeChargeCoeff * size * sizeChargeCurve.Evaluate(chargeTime);
                size = Mathf.Max(size, 1f); // size shouldn't go below 1

                if (size > 1)
                {
                    spinRateCharge += chargeIncrement * spinDirection * Time.deltaTime * spinChargeCoeff * spinChargeCurve.Evaluate(chargeTime);
                    impulseMagnitudeCharge += chargeIncrement * Time.deltaTime * impulseChargeCoeff;
                    projectileCharge += chargeIncrement * Time.deltaTime * projectileSpeedCoeff * projectileChargeCurve.Evaluate(chargeTime);
                }
            }
        }
        if (Input.GetKeyUp(actionKey))
        {
            SetSpeedInDirection(angle);
            FireProjectile();

            // Reset charge variables
            isCharging = false;
            chargeTime = 0f;
            impulseMagnitudeCharge = 0f;
            spinRateCharge = 0f;
            projectileCharge = 0f;
        }

        // Check if the action key is pressed
        //if (Input.GetKeyDown(actionKey))
        //{
        //    SetSpeedInDirection(angle);
        //}
        //if (Input.GetKeyDown("p"))
        //{
        //    //shrink(1);
        //    Damage();
        //}
    }

    // Function to apply an impulse in the direction of the current angle
    private void ApplyImpulseInDirection(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;

        // Calculate the direction vector (normalized) based on the angle
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Apply an impulse to the Rigidbody2D in the calculated direction
        rb.AddForce(direction * impulseMagnitude, ForceMode2D.Impulse);
    }

    // Function to set the speed in the direction of the current angle
    private void SetSpeedInDirection(float angle)
    {
        // Convert the angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate the direction vector (normalized) based on the angle
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Set the Rigidbody2D velocity in the calculated direction
        rb.linearVelocity = direction * (impulseMagnitude + impulseMagnitudeCharge);
    }

    private void FireProjectile()
    {
        float projectileDistance = transform.localScale.x/2 + 1f;
        Vector3 startPos = this.transform.position + 
                            new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * projectileDistance, Mathf.Sin(angle * Mathf.Deg2Rad) * projectileDistance, 0f);
        
        Bullet projectile = Instantiate(projectilePrefab, startPos, Quaternion.identity);
        // setting owner should prevent the player from hurting themselves
        projectile.SetOwner(this);
        projectile.Fire(projectileStrength, projectileCharge, angle, projectileGravity);
    }

    // allow others to shrink this players bubble by specified amount
    public void shrink (float amount) 
    {
        // make player grow
        size -= growRate * amount;

        // Don't go below 1
        size = Mathf.Max(size, 1f);
        
        // make player float
        rb.gravityScale += floatRate * amount;
    }

    public void Damage(float strength)
    {
        if (isDamaged)
        {
            return;
        }

        isDamaged = true;
        shrink(strength);

        // Change arrow spinning direction
        spinDirection *= -1f;

        spriteAnimation.Play("A_SpriteDamageWiggle");
        spriteAnimation.PlayQueued("A_SpriteFlash");

        StartCoroutine("ResetDamage");
    }

    IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(invulnTimeAfterDamage);

        isDamaged = false;
        spriteAnimation.Stop("A_SpriteFlash");
    }
}
