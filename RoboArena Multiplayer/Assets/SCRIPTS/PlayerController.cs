using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Realtime;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float slowSpeed;
    public float actualSpeed;
    public float dashSpeed;
    private Vector2 move;

    public Collider head;
    public Collider Torso;

    public InputActionReference ShieldAction; // Reference to the Input Action
    public bool ShieldIsUp; // Boolean to track if the button is held

    public Animator shield;

    PhotonView view;
    Player player;

    public float rotationSpeed;

    public void SetPlayerInfo(Player _player)
    {
        player = _player;
    }

    private void OnEnable()
    {
        // Enable the Input Action
        ShieldAction.action.Enable();
    }

    private void OnDisable()
    {
        // Disable the Input Action
        ShieldAction.action.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        actualSpeed = speed;
    }

    public void Shield()
    {
        actualSpeed = slowSpeed;

    }
    public void ShieldOff()
    {
        actualSpeed = speed;

    }
    public void Dash()
    {

            if (DashScript.instance.Dashing && !ShieldIsUp)
            {
                actualSpeed = dashSpeed;
            }


    }



    // Update is called once per frame
    void Update()
    {

        if (view.IsMine)
        {

            movePlayer();

            if (ShieldIsUp && DashScript.instance.Dashing == false)
            {
                Shield();
                print("Shield");
                shield.SetBool("Shield", true);
                head.enabled = false;
                Torso.enabled = false;
            }
            
            if(ShieldIsUp == false)
            {
                ShieldOff();
                shield.SetBool("Shield", false);
                head.enabled = true;
                Torso.enabled = true;
            }

            Dash();



        }
        float buttonValue = ShieldAction.action.ReadValue<float>();

        // Set the boolean based on the button value
        ShieldIsUp = buttonValue > 0;
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        transform.Translate(movement * actualSpeed * Time.deltaTime, Space.World);
    }

    public void moveArm()
    {
        // Implement arm movement if needed
    }
}
