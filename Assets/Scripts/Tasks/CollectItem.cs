using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    [SerializeField]
    CleanupTask task;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            task.itemCollected(this);
            gameObject.SetActive(false);
        }
    }
}
