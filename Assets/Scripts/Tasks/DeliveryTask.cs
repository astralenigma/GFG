using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTask : Task
{
    [SerializeField]
    EscortItem item;
    [SerializeField]
    Transform itemFinalDestiny;
    bool itemInTransit = false;
    /// <summary>
    /// Sets the information that the item has been collected and is in transit now.
    /// </summary>
    public void itemCollected()
    {
        itemInTransit = true;
    }
    /// <summary>
    /// Triggers that the item has been delivered and the Task Finished.
    /// </summary>
    public void itemDelivered()
    {
        TaskFinished();
    }
    /// <summary>
    /// Procedure to be called by the destiny trigger location.
    /// </summary>
    /// <param name="player">Player that reached destination to deal with item trade.</param>
    public void DestinationReached(Player player)
    {
        if (itemInTransit)
        {
            player.RemoveCarriedItem();
            itemDelivered();

        }
        if(itemFinalDestiny)
        {
            item.transform.SetParent(itemFinalDestiny, false);
        }
        else
        {
            item.gameObject.SetActive(false);
            Destroy( item.gameObject);
        }
        
        //item.transform.position=destiny.position;
    }

    public override void SetupTask()
    {
        item.gameObject.SetActive(true);
    }
}
