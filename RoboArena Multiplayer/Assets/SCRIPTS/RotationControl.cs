using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Realtime;
using Photon.Pun;


public class RotationControl : MonoBehaviour
{
    public InputActionReference joystickAction; // Reference to the joystick input action
    public float rotationSpeed = 100f; // Adjust this value as needed
    public float extensionFactor = 1.5f; // Adjust this value to extend the arm further
    public Vector2 lastJoystickInput; // Store the last valid joystick input

    PhotonView view;

    public TrailRenderer trail;

    public void Start()
    {
        view = GetComponent<PhotonView>();

    }

    void Update()
    {

        if (view.IsMine)
        {
            // Read joystick input
            Vector2 joystickInput = joystickAction.action.ReadValue<Vector2>();
            if (joystickInput != Vector2.zero)
            {
                lastJoystickInput = joystickInput; // Update last valid joystick input

                // Calculate rotation angles based on joystick input
                float angleX = Mathf.Atan2(joystickInput.y, 1) * Mathf.Rad2Deg * extensionFactor;
                float angleZ = Mathf.Atan2(joystickInput.x, 1) * Mathf.Rad2Deg * extensionFactor;

                // Apply rotation to the arm
                transform.rotation = Quaternion.Euler(angleX, 0f, -angleZ);

                trail.enabled = true;
            }
            else
            {
                // Calculate rotation angles based on last valid joystick input
                float angleX = Mathf.Atan2(lastJoystickInput.y, 1) * Mathf.Rad2Deg;
                float angleZ = Mathf.Atan2(lastJoystickInput.x, 1) * Mathf.Rad2Deg;

                // Apply rotation to the arm
                transform.rotation = Quaternion.Euler(angleX, 0f, -angleZ);

                trail.enabled = false;
            }

        }
    }

}

