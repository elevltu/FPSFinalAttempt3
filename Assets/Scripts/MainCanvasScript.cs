using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MainCanvasScript : NetworkBehaviour
{
    public TextMeshProUGUI health;
    private GameObject player;
    private GameObject[] players;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject j in players)
        {
            if (IsOwner)
            {
                player = j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject j in players)
        {
            if (IsOwner)
            {
                player = j;
            }
        }
        health.text = player.GetComponent<PlayerNetwork>().health.ToString();
    }
}
