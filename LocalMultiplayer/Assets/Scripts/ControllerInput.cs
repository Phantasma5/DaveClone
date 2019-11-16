using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ControllerInput : MonoBehaviour
{
    public string playerName;
    
    void Update()
    {
        if (Input.GetButtonDown(playerName + "Fire1"))
        {
            GetComponent<Movement>().MoveRight();
        }
        //GameManager.instance.
    }
}
