using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BombSpawn : MonoBehaviourPunCallbacks
{
    public GameObject GameItem;
    public GameObject Wall;



    /*public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;*/
    public List<Transform> spawnPoints = new List<Transform>();

    public List<Transform> spawnPointsWalls = new List<Transform>();


    private bool spawn;

    public static BombSpawn instance;

    void Start()
    {



        instance = this;

        if (PhotonNetwork.IsMasterClient)
        {
            Spawn();


        }
    }

    public void SpawnWalls()
    {
        {
            StartCoroutine(SpawnWallsCoroutine());
        }
    }

    public void Spawn()
    {
        print("spawn");

        {
            StartCoroutine(SpawnBombCoroutine());
        }
    }

    IEnumerator SpawnWallsCoroutine()
    {

        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            int randomIndexWall = Random.Range(0, spawnPointsWalls.Count);
            Transform spawnPointWall = spawnPointsWalls[randomIndexWall];
            if (spawnPointWall != null)
            {
                PhotonNetwork.Instantiate(Wall.name, spawnPointWall.position, Quaternion.identity);
                break;
            }
            else
            {
                continue;
            }
        }
    }


    IEnumerator SpawnBombCoroutine()
    {


        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];
            if (spawnPoint != null)
            {
                PhotonNetwork.Instantiate(GameItem.name, spawnPoint.position, Quaternion.identity);
                break;
            }
            else
            {
                continue;
            }
        }
    }

    public void RemoveSpawnPoint(Transform spawnPoint)
    {
        spawnPoints.Remove(spawnPoint);
        print("spawnpointRemoved");
    }
}
