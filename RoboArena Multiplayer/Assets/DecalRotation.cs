using UnityEngine;
using UnityEngine.InputSystem;

public class RotateDecal : MonoBehaviour
{
    public float rotationSpeed = 100f; // Adjust the rotation speed as needed
    public Transform decal; // Reference to the decal object

    private Vector2 joystickInput;

    public void OnRotateArm(InputAction.CallbackContext context)
    {
        joystickInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (joystickInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(joystickInput.x, joystickInput.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            decal.rotation = Quaternion.RotateTowards(decal.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
