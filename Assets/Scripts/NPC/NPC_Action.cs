using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Action : MonoBehaviour
{
    bool player_detection = false;

    void Update()
    {
        if (player_detection && Input.GetKeyDown(KeyCode.F))
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.name == "PLayerBody")
        {
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player_detection = false;
    }
}
