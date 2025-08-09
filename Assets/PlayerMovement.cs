using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Vector2 touchStart;

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // Keyboard input through the new Input System
        if (Keyboard.current != null)
        {
            float moveX = 0f;
            float moveZ = 0f;

            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
                moveX -= 1f;
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
                moveX += 1f;
            if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
                moveZ -= 1f;
            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
                moveZ += 1f;

            movement += new Vector3(moveX, 0, moveZ);
        }

        // Touch input for mobile devices
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.touches[0];

            var phase = touch.phase.ReadValue();
            if (phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                touchStart = touch.position.ReadValue();
            }
            else if (phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                     phase == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                Vector2 touchCurrent = touch.position.ReadValue();
                Vector2 direction = touchCurrent - touchStart;
                direction.Normalize();

                movement = new Vector3(direction.x, 0, direction.y);
            }
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
