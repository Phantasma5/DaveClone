using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRPCTest : MonoBehaviour
{
    public void SetScale(Vector3 aScale, string a, int b)
    {
        gameObject.transform.localScale = aScale;
    }
}
