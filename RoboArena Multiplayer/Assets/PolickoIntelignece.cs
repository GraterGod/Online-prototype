using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PolickoIntelignece : MonoBehaviour
{
    public string Tag1;
    public string Tag2;

    public float TimeBetweenPic;
    public bool Pick;

    public MeshRenderer meshRenderer;

    public Material material1;
    public Material material2;

    PhotonView view;

    public BombSpawn bombSpawn;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        Pick = true;
        bombSpawn = FindObjectOfType<BombSpawn>();
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void DestroyThis()
    {
        if(bombSpawn != null)
        {
            print("2");
            bombSpawn.spawnPoints.Remove(transform);
        }

        view.RPC("DestroyRPC", RpcTarget.All);
    }
    
    [PunRPC]
    public void DestroyRPC()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    public void CanRPC()
    {
        transform.gameObject.tag = Tag1;
        meshRenderer.material = material1;
        Pick = true;
    }

    [PunRPC]
    public void CantRPC()
    {
        transform.gameObject.tag = Tag2;
        meshRenderer.material = material2;
        Pick = true;
    }

    public void RandomPic()
    {
        if (view.IsMine)
        {
            int randomIndex = Random.Range(0, 2);

            if(randomIndex == 0)
            {
                view.RPC("CanRPC", RpcTarget.All);
            }
            else
            {
                view.RPC("CantRPC", RpcTarget.All);
            }
        } 
    }


    public IEnumerator BetweenPic()
    {
        
        Pick = false;
        yield return new WaitForSeconds(TimeBetweenPic);
        RandomPic();
    }

    // Update is called once per frame
    void Update()
    {
        
            if (Pick)
            {
                StartCoroutine(BetweenPic());
            }

       
    }
}
