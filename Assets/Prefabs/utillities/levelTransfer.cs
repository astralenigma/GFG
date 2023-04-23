using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelTransfer : MonoBehaviour
    
{
    public Transform destiny;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = destiny.position;
            other.transform.rotation = destiny.rotation;
        }
        
    }
}
