using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTask : Task
{
    [SerializeField]
    EscortItem item;
    bool itemInTransit=false;
    public void itemCollected(EscortItem item)
    {
        itemInTransit = true;
    }

    public void itemDelivered() {
        TaskFinished();
    }

    public override void SetupTask()
    {
        item.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (itemInTransit)
        {
            if (!other.CompareTag("Player")) {
                GetComponent<Player>().SetCarriedItem(null);
                itemDelivered();
            }
        }
    }
}
