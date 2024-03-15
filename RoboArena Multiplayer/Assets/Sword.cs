using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public ParticleSystem blood;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            blood.Play();
        }

        if (other.CompareTag("Torso"))
        {
            blood.Play();

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
