using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour{
    [SerializeField] private Button serverButton; //connect server button here
    [SerializeField] private Button hostButton; //connect host button here
    [SerializeField] private Button clientButton; //connect client button here
    public static bool isConnected = false;
    private void Awake()
    {
        
        // watch https://www.youtube.com/watch?v=3ZfwqWl-YI0&t=0s later to understand
        serverButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            isConnected = true;
        });
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            isConnected = true;
        });
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            isConnected = true;
        });
    }
}
