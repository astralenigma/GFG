using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransfer : MonoBehaviour
    
{
    public Transform destiny;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = destiny.position;
            other.transform.rotation = destiny.rotation;
        }
        
    }
}
