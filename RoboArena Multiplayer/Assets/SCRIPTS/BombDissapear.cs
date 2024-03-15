using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BombDissapear : MonoBehaviourPunCallbacks
{

    CharacterCompletController[] Players;
    CharacterCompletController nearestPlayer;

    PhotonView view;
    bool canDestroy;
    public static BombDissapear instance;

    public void Start()
    {
        Players = FindObjectsOfType<CharacterCompletController>();
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void collect()
    {
        
        if(nearestPlayer != null)
        {
            print("collect");
            nearestPlayer.GetComponent<CharacterCompletController>().totalThrows++;
        }

    }

    public void DestroyExisting()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    public void Update()
    {
        if (Players.Length == 1)
        {
            // If there's only one player, set it as the nearest player
            nearestPlayer = Players[0];
        }
        else if (Players.Length > 1)
        {
            // Calculate distances only if there are more than one player
            float distanceOne = Vector2.Distance(transform.position, Players[0].transform.position);
            float distanceTwo = Vector2.Distance(transform.position, Players[1].transform.position);

            // Determine the nearest player
            if (distanceOne < distanceTwo)
            {
                nearestPlayer = Players[0];
            }
            else
            {
                nearestPlayer = Players[1];
            }
        }

    }

}
