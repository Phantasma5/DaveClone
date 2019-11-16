using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int totalPlayers;
    class Player
    {
        string playerName;
        int colorOption;
    }
    List<Player> players;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            // Make sure we can access from anywhere
            instance = this;
        }
        else
        {
            // Make sure only one exists
            Destroy(gameObject);
        }
    }
}
