using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyRestoreLocation : MonoBehaviour
{
    public float energyRestoreRate=2;
    private void Start()
    {
        GameManager.Instance.restoreLocations.Add(this);
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.RestoreEnergy(energyRestoreRate);
        }
    }
}
