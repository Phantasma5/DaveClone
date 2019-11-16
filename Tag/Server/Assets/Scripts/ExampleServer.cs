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
    // Display dummy network objects locally
    [SerializeField]
    GameObject netObjDemo;
    Dictionary<int, GameObject> netObjs = new Dictionary<int, GameObject>();
    bool canTag = true;
    float tagbackTime = 1.0f;

    // Game logic
    // Which networked game object is currenty it
    int objectCurrentlyIt = 0;


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
        if (objectCurrentlyIt != 0)
        {
            serverNet.CallRPC("SetItState", info.id, objectCurrentlyIt, true);
        }
    }

    public void Update()
    {
        Dictionary<int, ServerNetwork.NetworkObject> objInfo = serverNet.GetAllObjects();
        foreach (KeyValuePair<int, ServerNetwork.NetworkObject> objKV in objInfo)
        {
            netObjs[objKV.Key].transform.position = objKV.Value.position;
        }

        Dictionary<int, ServerNetwork.NetworkObject> objs = serverNet.GetAllObjects();
        if (objectCurrentlyIt == 0 || !objs.ContainsKey(objectCurrentlyIt))
        {
            if (objs.Count == 0)
            {
                return;
            }

            // Pick someone "random" and tell them they are it
            foreach (KeyValuePair<int, ServerNetwork.NetworkObject> objData in objs)
            {
                if (objData.Value.prefabName == "Player")
                {
                    objectCurrentlyIt = objData.Value.networkId;
                    serverNet.CallRPC("SetItState", UCNetwork.MessageReceiver.AllClients,
                        objData.Value.networkId, true);
                    return;
                }
            }
        }

        // Check if someone has been "tagged"
        if (objs.Count > 1 && canTag)
        {
            ServerNetwork.NetworkObject itObj = objs[objectCurrentlyIt];
            foreach (KeyValuePair<int, ServerNetwork.NetworkObject> objData in objs)
            {
                if (objData.Value.prefabName == "Player")
                {
                    if (objData.Value.networkId == objectCurrentlyIt)
                    {
                        continue;
                    }
                    float sqrDis = (itObj.position - objData.Value.position).sqrMagnitude;
                    if (sqrDis < 1)
                    {
                        serverNet.CallRPC("SetItState", UCNetwork.MessageReceiver.AllClients,
                            objectCurrentlyIt, false);
                        serverNet.CallRPC("SetItState", UCNetwork.MessageReceiver.AllClients,
                            objData.Value.networkId, true);
                        objectCurrentlyIt = objData.Value.networkId;

                        canTag = false;
                        StartCoroutine(TagbackTime());
                        return;
                    }
                }
            }
        }
    }

    IEnumerator TagbackTime()
    {
        yield return new WaitForSeconds(tagbackTime);
        canTag = true;
    }

    void OnInstantiateNetworkObject(ServerNetwork.IntantiateObjectData data)
    {
        GameObject newObj = Instantiate(netObjDemo);
        netObjs.Add(data.netObjId, newObj);
    }

    void OnDestroyNetworkObject(int aObjId)
    {
        if (netObjs.ContainsKey(aObjId))
        {
            Destroy(netObjs[aObjId]);
            netObjs.Remove(aObjId);
        }

        if (aObjId == objectCurrentlyIt)
        {
            objectCurrentlyIt = 0;
        }
    }
}
