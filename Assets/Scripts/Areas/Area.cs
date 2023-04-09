using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Area : MonoBehaviour
{

    private AreaTrigger areaTrigger;

    public void Init(AreaTrigger areaTrigger)
    {
        this.areaTrigger = areaTrigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered !!");
        if (other.CompareTag("Player") && areaTrigger.onEnter != null)
        {
            areaTrigger.onEnter.Invoke();

            // Change lighting and fog here
        }
    }

}
