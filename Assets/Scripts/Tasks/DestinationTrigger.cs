using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DestinationTrigger : MonoBehaviour
{
    [SerializeField]
    DeliveryTask task;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            task.DestinationReached(other.GetComponent<Player>());
        }
    }
}
