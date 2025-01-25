using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public string actionKey = "space";
    [SerializeField]
    private float spinRate = 1;   
    [SerializeField]
    private float growRate = 1; 
    [SerializeField]
    private float floatRate = .1f; 
    [SerializeField]
    private float impulseMagnitude = 5f; 
    [SerializeField]
    private float startingGravityScale = 0.001f; 

    private float angle = 0;  
    private float size = 1;       

    private ArrowDrawer arrowDrawer;
    private Rigidbody2D rb;            

    // Start is called once before the first execution of Update
    void Start()
    {
        arrowDrawer = GetComponent<ArrowDrawer>();
        rb = GetComponent<Rigidbody2D>();  

        rb.gravityScale = startingGravityScale;      
    }

    // Update is called once per frame
    void Update()
    {
        // make control arrow spin
        angle += spinRate * Time.deltaTime;
        // make player grow
        size += growRate * Time.deltaTime;
        // make player float
        rb.gravityScale -= floatRate * Time.deltaTime;


        // update size
        transform.localScale = Vector3.one * size;
        // Draw the arrow at the current angle and size
        float arrowDistance = transform.localScale.x/2 + 0.3f;
        arrowDrawer.DrawArrow(angle, arrowDistance);

        // Check if the action key is pressed
        if (Input.GetKeyDown(actionKey))
        {
            SetSpeedInDirection(angle);
        }
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
        rb.linearVelocity = direction * impulseMagnitude;
    }
}

