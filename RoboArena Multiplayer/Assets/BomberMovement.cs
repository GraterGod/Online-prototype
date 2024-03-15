using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Realtime;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody))]
public class BomberMovement : MonoBehaviour
{
    public float speed;

    [Tooltip("Dash Speed is 50")]
    public float DashingTime;

    private Vector2 move, mouseLook, joystickLook;
    private Vector3 rotationTarget;
    float dashSpeed = 50;

    float DashTrigger; 
    public bool isPC;

    public bool dashing;
    public float basicSpeed;
    public bool lowThrow;

    public PhotonView view;
    Player player;



    public static BomberMovement instance;



    public void SetPlayerInfo(Player _player)
    {
        player = _player;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnleftDash(InputAction.CallbackContext context)
    {
        if (view.IsMine)
        {
            //DashTrigger = context.ReadValue<float>(); // toto keby chces štít držat => proste držíš tlaèidlo
            if (context.performed && dashing == false)
            {
                StartCoroutine(DashTime());
            }
        }
    }

    public IEnumerator DashTime()
    {
        dashing = true;
        lowThrow = true;
        yield return new WaitForSeconds(DashingTime);
        dashing = false;
        yield return new WaitForSeconds(.3f);
        lowThrow = false;
    }

    public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }





    void Start()
    {
        basicSpeed = speed;
        view = GetComponent<PhotonView>();
        instance = this;
    }



    void Update()
    {

        if (view.IsMine)
        {
            


            if (Input.GetKeyDown(KeyCode.F5))
            {
                isPC = true;
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                isPC = false;
            }

            bool button = DashTrigger > 0;
            if (button)
            {
                print("Dash");
            }

            if (dashing)
            {
                speed = dashSpeed;
            }
            else
            {
                speed = basicSpeed;
            }

            if (lowThrow)
            {
                print("lowThrow Avaliable");
            }
          


            if (isPC)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(mouseLook);

                if(Physics.Raycast(ray, out hit))
                {
                    rotationTarget = hit.point;
                }

                movePlayerWithAim();
            }
            else
            {
                if(joystickLook.y == 0 && joystickLook.y == 0)
                {
                    movePlayer();
                }
                else
                {
                    movePlayerWithAim();
                }
            }
        }
    }

    public void movePlayer()
    {
        
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    public void movePlayerWithAim()
    {
        if (isPC)
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if(aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
            }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);
            
            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f);
            }
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}

