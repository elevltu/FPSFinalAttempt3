using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour{
    [SerializeField] private Button serverButton; //connect server button here
    [SerializeField] private Button hostButton; //connect host button here
    [SerializeField] private Button clientButton; //connect client button here
    
    
    private void Awake()
    {
        GameObject emptyObject = GameObject.Find("NetworkManagerUI");
        // watch https://www.youtube.com/watch?v=3ZfwqWl-YI0&t=0s later to understand
        serverButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            emptyObject.SetActive(false);
        });
        hostButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.DisconnectClient(NetworkManager.ServerClientId);
            }
            NetworkManager.Singleton.StartHost();
            emptyObject.SetActive(false);
        });
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            emptyObject.SetActive(false);
        });
    }
}
