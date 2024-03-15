using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CameraScript : MonoBehaviourPunCallbacks
{
    public GameObject cameraHolder;
    public Vector3 offset;

    private PhotonView photonView;



    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            cameraHolder.SetActive(true);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Map_01")
        {
            cameraHolder.transform.position = transform.position + offset;
        }
    }
}
