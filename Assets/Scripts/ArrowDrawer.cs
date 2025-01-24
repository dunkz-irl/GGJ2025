using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    public GameObject arrowPrefab; // Reference to the arrow sprite prefab

    private GameObject arrowInstance;

    void Start()
    {
        // Create and instantiate the arrow sprite (it can be created as a prefab and assigned in the Inspector)
        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow prefab is not assigned.");
            return;
        }

        arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrowInstance.SetActive(false); // Start with the arrow hidden
    }

    // Function to draw the arrow given an angle and start distance
    public void DrawArrow(float angle, float startDistance)
    {
        // Activate the arrow sprite
        arrowInstance.SetActive(true);

        // Calculate the start position, offset from the center by startDistance
        Vector3 centerPosition = transform.position;
        Vector3 startPosition = centerPosition + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * startDistance, Mathf.Sin(angle * Mathf.Deg2Rad) * startDistance, 0f);

        // Position the arrow at the start position
        arrowInstance.transform.position = startPosition;

        // Rotate the arrow to face the correct direction
        float rotationAngle = angle - 90f; // Adjust because Unity's sprite rotation is 90 degrees off
        arrowInstance.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }

    // For testing, call this in the Update function
    void Update()
    {
        DrawArrow(45f, 1f); // Example: Draw an arrow at a 45-degree angle, starting 1 unit away from the object
    }
}
