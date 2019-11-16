using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTest : MonoBehaviour
{
    [SerializeField]
    List<GameObject> networkPrefabs;
    

    public GameObject InstantiatePrefab(string aName)
    {
        foreach (GameObject go in networkPrefabs)
        {
            if (go.name == aName)
            {
                return Instantiate(go);
            }
        }
        return null;
    }

}
