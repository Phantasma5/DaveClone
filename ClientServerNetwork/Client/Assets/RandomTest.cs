using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTest : MonoBehaviour
{
    int numRand = 10;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (numRand > 0)
        {
            numRand--;
            Debug.Log(Random.Range(0, 10));
        }
    }
}
