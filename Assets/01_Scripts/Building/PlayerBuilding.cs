using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    public float buildingDistance;
    public LayerMask buildingLayer;

    [Header("For testing")]
    public int buildIndex;

    private void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.red, 10f);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, buildingDistance, buildingLayer) && Input.GetKeyDown(KeyCode.E))
        {
            
            if (hit.transform.gameObject.GetComponent<Buildable>() != null)
            {
                Buildable buildable = hit.transform.gameObject.GetComponent<Buildable>();
                foreach (Buildable.Component comp in buildable.buildComponents)
                {
                    if (comp.componentIndex == buildIndex)
                    {
                        if(comp.currentAmount< comp.neededAmount)
                        {
                            comp.currentAmount++;
                        }
                    }
                    CheckIfBuildableFinished(buildable);
                }
            }
        }
    }
   
    private void CheckIfBuildableFinished(Buildable buildable)
    {
        foreach (Buildable.Component comp in buildable.buildComponents)
        {
            if (comp.currentAmount < comp.neededAmount)
                return;
        }
        buildable.BuildObject();
    }
}
