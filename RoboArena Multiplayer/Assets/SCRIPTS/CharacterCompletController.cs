using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Photon.Realtime;
using Photon.Pun;

public class CharacterCompletController : MonoBehaviour
{
    [Header("Movement")]
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

    Player player;

    [Header("References")]
    public Transform Direction;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float CoreThrowForce;
    public float throwUpwardForce;
    public float CoreThrowUpForce;
    public float DashThrowForce;

    public bool cantThrow;


    public bool readyToThrow;
    public static CharacterCompletController Instance;

    public PhotonView view;

    public GameObject BombBarer;

    public ParticleSystem Explosion;

    public BombSpawn bombSpawn;
    public BombDissapear bombDissapear;
    

    [Header("CharacterHitpoint")]
    public Transform PlayerTransform;
    public float HitRadius;
    public Vector3 offsetGizmos = Vector3.zero;
    bool[] playersWithinRadius;
    bool isInHitpoint;


    void Start()
    {
        Instance = this;
        readyToThrow = true;
        view = GetComponent<PhotonView>();
        bombSpawn = FindObjectOfType<BombSpawn>();
        basicSpeed = speed;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Initialize the array to keep track of players within the radius
        playersWithinRadius = new bool[players.Length];
    }

    public void SetPlayerInfo(Player _player)
    {
        player = _player;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnHeadReset(InputAction.CallbackContext context)
    {
        if (view.IsMine)
        {
            if (context.performed)
            {
                view.RPC("RespawnHead", RpcTarget.All);
            }
        }
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

    [PunRPC]
    void RespawnHead()
    {
        bombSpawn.Spawn();
    }

    public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update

    [PunRPC]
    public void ExplosionRPC()
    {
        Explosion.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 gizmoPosition = transform.position + offsetGizmos;
        Gizmos.DrawWireSphere(gizmoPosition, HitRadius);
    }

    public void OnThrow(InputAction.CallbackContext context)
    {

        if (view.IsMine)
        {

            if (context.performed && readyToThrow && totalThrows > 0 && cantThrow == false)
            {
                Throw();
            }

            /*if (context.performed && readyToThrow && totalThrows > 0 && cantThrow)
            {
                view.RPC("ExplosionRPC", RpcTarget.All);
                totalThrows--;
                BombSpawn.instance.Spawn();

            }*/

            /*if (context.performed && readyToThrow && totalThrows > 0 && cantThrow == false && BomberMovement.instance.lowThrow)
            {
                LowThrow();
            }*/

        }
    }
    private void OnCollisionStay(Collision collision)
    {

        if (view.IsMine)
        {
            if (collision.gameObject.CompareTag("CantThrowGround"))
            {
                view.RPC("CantThrowRPC", RpcTarget.All);
            }

            if (collision.gameObject.CompareTag("Ground"))
            {
                view.RPC("CanThrowRPC", RpcTarget.All);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (view.IsMine)
        {
            if (collision.gameObject.CompareTag("CantThrowGround"))
            {
                view.RPC("CanThrowRPC", RpcTarget.All);
            }

            if (collision.gameObject.CompareTag("Ground"))
            {
                view.RPC("CantThrowRPC", RpcTarget.All);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (view.IsMine)
        {
            if (other.CompareTag("HitPoint") && totalThrows > 0) 
            {
                totalThrows--;
                bombSpawn.Spawn();
            }

            if (other.CompareTag("BombCollect"))
            {
                BombDissapear.instance.collect();

            }
        }
    }
    
    


    [PunRPC]
    void CantThrowRPC()
    {
        cantThrow = true;
    }

    [PunRPC]
    void CanThrowRPC()
    {
        cantThrow = false;
    }

    public void LowThrow()
    {
        print("lowThrow");
        throwUpwardForce = 0;
        throwForce = DashThrowForce;
    }

    public void NormalThrow()
    {
       throwUpwardForce = CoreThrowUpForce;
       throwForce = CoreThrowForce;
    }




    public void Throw()
    {

            readyToThrow = false;

            GameObject projectile = PhotonNetwork.Instantiate(objectToThrow.name, attackPoint.position, Direction.rotation);

            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            Vector3 forceToAdd = Direction.transform.forward * throwForce + transform.up * throwUpwardForce;

            projectileRigidbody.AddForce(forceToAdd, ForceMode.Impulse);

            totalThrows--;

            Invoke(nameof(ResetThrow), throwCooldown);

        
    }

    public void loosingHead()
    {

            if(totalThrows > 0)
            {
                totalThrows--;
                isInHitpoint = false;
                bombSpawn.Spawn();

            }

    }

    public void IsInHitRadius()
    {
        print("is in Hit Radius");
    }

    private void ResetThrow()
    {
 
             readyToThrow = true;

    }

    [PunRPC]
    void BombBarerTrueRPC()
    {
        BombBarer.SetActive(true);

    }

    [PunRPC]
    void BombBarerFalseRPC()
    {
        BombBarer.SetActive(false);
    }
    // Update is called once per frame
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

            if (isInHitpoint)
            {
                loosingHead();
            }

            if (isPC)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(mouseLook);

                if (Physics.Raycast(ray, out hit))
                {
                    rotationTarget = hit.point;
                }

                movePlayerWithAim();
            }
            else
            {
                if (joystickLook.y == 0 && joystickLook.y == 0)
                {
                    movePlayer();
                }
                else
                {
                    movePlayerWithAim();
                }
            }

            if (totalThrows != 0)
            {
                view.RPC("BombBarerTrueRPC", RpcTarget.All);
            }
            else
            {
                view.RPC("BombBarerFalseRPC", RpcTarget.All);
            }

            if (lowThrow)
            {
                LowThrow();
            }
            else
            {
                NormalThrow();
            }

            if(totalThrows > 1)
            {
                totalThrows = 1;
            }

            ///
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            // Ensure that playersWithinRadius has the same length as players array
            if (playersWithinRadius == null || playersWithinRadius.Length != players.Length)
            {
                playersWithinRadius = new bool[players.Length]; // Initialize playersWithinRadius with the same length as players
            }

            // Iterate through all players
            for (int i = 0; i < players.Length; i++)
            {
                Vector3 gismoPosition = transform.position + offsetGizmos;
                // Calculate the distance between this player and the gizmo
                float distance = Vector3.Distance(gismoPosition, players[i].transform.position);

                // Check if the player is within the gizmo radius
                if (distance <= HitRadius)
                {
                    // Check if the player was not previously within the radius
                    if (!playersWithinRadius[i])
                    {
                        // Player just entered the gizmo radius
                        Debug.Log("Player " + players[i].name + " entered the gizmo radius.");
                        isInHitpoint = true;
                    }
                    playersWithinRadius[i] = true; // Update the player's status within the radius
                }
                else
                {
                    // Check if the player was previously within the radius
                    if (playersWithinRadius[i])
                    {
                        // Player just exited the gizmo radius
                        Debug.Log("Player " + players[i].name + " exited the gizmo radius.");
                        isInHitpoint = false;

                    }
                    playersWithinRadius[i] = false; // Update the player's status within the radius
                }
            }


        }


    }
        public void movePlayer()
        {

            Vector3 movement = new Vector3(move.x, 0f, move.y);

            if (movement != Vector3.zero)
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

                if (aimDirection != Vector3.zero)
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
