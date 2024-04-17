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
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("PlayerCamera");
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject j in players)
        {
            if (IsLocalPlayer)
            {
                player = j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = player.transform.position;
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
        cameraRotation.y +=Input.GetAxis("Mouse X");
        camera.transform.eulerAngles = (Vector2)cameraRotation * 1;
    }
}
