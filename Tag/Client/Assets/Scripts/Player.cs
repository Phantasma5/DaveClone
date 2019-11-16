using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // Current speed of the player
    float speed = 5;

    [SerializeField]
    float itSpeed = 7;
    [SerializeField]
    float notItSpeed = 5;

    // Am I currently "it"
    bool isIt = false;

    public void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<NetworkSync>().owned)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0);
            transform.position += movement;
        }
    }
    void OnGainOwnership()
    {
        SetItState(false);
    }

    public void SetItState(bool aIsIt)
    {
        isIt = aIsIt;
        if (aIsIt)
        {
            // Logic for when I am it
            speed = itSpeed;
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            // Logic for when I am not it
            speed = notItSpeed;
            if (GetComponent<NetworkSync>().owned)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.gray;
            }
        }
    }
}
