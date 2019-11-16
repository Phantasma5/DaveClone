using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class ExampleServer : MonoBehaviour
{
    public static ExampleServer instance;

    public ServerNetwork serverNet;

    public int portNumber = 603;

    int objectCurrentIt;

    [SerializeField]
    GameObject netObjDemo;
    Dictionary<int, GameObject> netObjs = new Dictionary<int, GameObject>();

    // Use this for initialization
    void Awake()
    {
        instance = this;

        // Initialization of the server network
        ServerNetwork.port = portNumber;
        if (serverNet == null)
        {
            serverNet = GetComponent<ServerNetwork>();
        }
        if (serverNet == null)
        {
            serverNet = (ServerNetwork)gameObject.AddComponent(typeof(ServerNetwork));
            Debug.Log("ServerNetwork component added.");
        }

        //serverNet.EnableLogging("rpcLog.txt");
    }

    // A client has just requested to connect to the server
    void ConnectionRequest(ServerNetwork.ConnectionRequestInfo data)
    {
        Debug.Log("Connection request from " + data.username);

        serverNet.ConnectionApproved(data.id);

    }

    void OnAddArea(ServerNetwork.AreaChangeInfo info)
    {
        serverNet.CallRPC("SetWhoIsIt", info.id, objectCurrentIt);
    }

    public void Update()
    {
        Dictionary<int, ServerNetwork.NetworkObject> objInfo = serverNet.GetAllObjects();
        foreach (KeyValuePair<int, ServerNetwork.NetworkObject> objKV in objInfo)
        {
            netObjs[objKV.Key].transform.position = objKV.Value.position;
        }
    }

    void OnInstantiateNetworkObject(ServerNetwork.IntantiateObjectData data)
    {
        GameObject newObj = Instantiate(netObjDemo);

        netObjs.Add(data.netObjId, newObj);
    }
}
