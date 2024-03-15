using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GravityAcceleration : MonoBehaviour
{
    public float gravityScale;
    public Rigidbody rb;
    public GameObject particles;

    public BombSpawn bombSpawn;

    public static GravityAcceleration instance;

    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        instance = this;
        bombSpawn = FindObjectOfType<BombSpawn>();
    }



    public void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.collider.CompareTag("Gong"))
            {
                bombSpawn.SpawnWalls();
            }

        }
    }

    [PunRPC]
    public void Boom()
    {
        PhotonNetwork.Instantiate(particles.name, transform.position, Quaternion.identity);
        BombSpawn.instance.Spawn();
        PhotonNetwork.Destroy(this.gameObject);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);

    }
}
