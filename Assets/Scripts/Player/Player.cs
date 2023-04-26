using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField]
    Transform itemHold;
    EscortItem carriedItem;

    /// <summary>
    /// if the player isn't carrying an item it sets item as being carried and moves it to the player's itemHold location.
    /// </summary>
    /// <param name="escortItem">Item to be set as being carried.</param>
    /// <returns></returns>
    public bool SetCarriedItem(EscortItem escortItem)
    {
        if (carriedItem != null)
        {
            return false;
        }
        carriedItem = escortItem;
        if (carriedItem != null ) {
            carriedItem.transform.SetParent(itemHold,false);
        }
        //else
        //{
        //    Destroy(carriedItem.gameObject);
        //    carriedItem = null;
        //}
        return true;
    }
    /// <summary>
    /// Removes the carried item.
    /// </summary>
    public void RemoveCarriedItem()
    {
        //Destroy(carriedItem.gameObject );
        carriedItem=null;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance == null)
        {
            GetComponent<PlayerMovement>().DebugPlayerMovement();
        }
        else
        {
            GameManager.Instance.SetPlayer(this);
        }
    }
}
