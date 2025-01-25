using UnityEngine;

public class RisingFloor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPostiion = transform.position;
        newPostiion.y = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y - 0.5f;
        transform.SetPositionAndRotation(newPostiion, transform.rotation);
    }
}
