using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 touchStart;

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // For testing in the Editor (keyboard input)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        movement += new Vector3(moveX, 0, moveZ);

        // If there's touch input, override with that
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // On touch begin, record starting position
            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
            }
            // On touch move, calculate direction
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector2 touchCurrent = touch.position;
                Vector2 direction = touchCurrent - touchStart;
                direction.Normalize(); // Get a normalized direction

                movement = new Vector3(direction.x, 0, direction.y);
            }
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
