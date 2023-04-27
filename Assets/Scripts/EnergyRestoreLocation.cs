using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyRestoreLocation : MonoBehaviour
{
    public float energyRestoreRate=2;
    bool gameManagerActive=true;
    private void Start()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.restoreLocations.Add(this);
        }
        else
        {
            Debug.LogWarning("GameManager not found!");
            gameManagerActive=false;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player")&&gameManagerActive)
        {
            GameManager.Instance.RestoreEnergy(energyRestoreRate);
        }
    }
}
