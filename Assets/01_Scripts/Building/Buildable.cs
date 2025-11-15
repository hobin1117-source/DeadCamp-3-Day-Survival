using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    public GameObject finishedObject;
    public List<Component> buildComponents = new List<Component>();



    public void BuildObject()
    {
        GameObject result = Instantiate(finishedObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    [System.Serializable]
    public class Component
    {
        public string componentName;
        public int componentIndex;

        public int currentAmount;
        public int neededAmount;

    }

}
