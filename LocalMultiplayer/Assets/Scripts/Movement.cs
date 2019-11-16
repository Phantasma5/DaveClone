using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5.0f;

    public void MoveRight()
    {
        transform.position = new Vector3(
                transform.position.x + speed * Time.deltaTime,
                transform.position.y,
                transform.position.z);
    }
}
