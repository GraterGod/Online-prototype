using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Realtime;
using Photon.Pun;

public class DashScript : MonoBehaviour
{
    public InputActionReference buttonDash; // Reference to the Input Action
    //PhotonView view;
    public bool Dashing;
    public float DashLength;

    public static DashScript instance;

    PhotonView view;


    private void Start()
    {
        instance = this;
        //view.GetComponent<PhotonView>();

        view = GetComponent<PhotonView>();

    }
    private void OnEnable()
    {
        // Enable the Input Action
        buttonDash.action.Enable();

        // Register the event for button press
        buttonDash.action.performed += OnButtonPressed;
    }

    private void OnDisable()
    {
        // Disable the Input Action
        buttonDash.action.Disable();

        // Unregister the event
        buttonDash.action.performed -= OnButtonPressed;
    }

    public IEnumerator DashTime()
    {
        Dashing = true;

        yield return new WaitForSeconds(DashLength);

        Dashing = false;
    }
    // Event handler for button press
    public void OnButtonPressed(InputAction.CallbackContext context)
    {
    
        Debug.Log("Dash");
        

        StartCoroutine(DashTime());
        // Add your code here to handle the button press event

        
    }
}