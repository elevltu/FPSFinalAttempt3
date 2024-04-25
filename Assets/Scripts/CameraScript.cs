using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraScript : NetworkBehaviour
{
    private GameObject player;
    private GameObject camera;
    private GameObject[] players;
    private Vector3 cameraRotation;
    private Vector3 cameraOffset = new Vector3 (0, 1, 0);
    private Vector3 originalPosition;
    public bool playerDead;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        playerDead = false;
        camera = this.gameObject;
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
        if (player != null)
        {
            camera.transform.position = player.transform.position + cameraOffset;
            cameraRotation.x += -Input.GetAxis("Mouse Y");
            if (cameraRotation.x > 90)
            {
                cameraRotation.x = 90;
            }
            if (cameraRotation.x < -90)
            {
                cameraRotation.x = -90;
            }
            {

            }
            cameraRotation.y += Input.GetAxis("Mouse X");
            camera.transform.eulerAngles = (Vector2)cameraRotation * 1;
        }
    }
}
