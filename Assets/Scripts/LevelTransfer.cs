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
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            movement.enabled=false;
            other.transform.position = destiny.position;
            other.transform.rotation = destiny.rotation;
            StartCoroutine(restartMovement(movement));
        }
        
    }
    IEnumerator restartMovement(PlayerMovement movement) {
        yield return new WaitForSeconds(0.1f);//WaitForEndOfFrame();
        movement.enabled=true;
    }
}
