using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    Vector2 rotation = Vector2.zero;
    Vector2 cameraRotation = Vector2.zero;
    private float lookSpeed = 1;
    private GameObject camera;
    [SerializeField]private GameObject[] cameras;
    private GameObject menuCamera;
    private Rigidbody rb;

    [SerializeField] private Transform spawnerPrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private Transform bulletPrefab;

    private int health;
    private float shootCooldown;
    private bool isOnCooldown;
    private bool cooldownRunning;

    private Transform player;
    private Vector3 cameraOffset = new Vector3(0, 1, 0);

    public override void OnNetworkSpawn() {
        menuCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (menuCamera != null)
        {
            menuCamera.SetActive(false);
        }

        camera = GameObject.FindGameObjectWithTag("PlayerCamera");
        //cameras[OwnerClientId].SetActive(true);
        /*int i;
        for ( i = 0; i < cameras.Length; i++)
        {
            //if (IsOwner)
            if (i != (int)OwnerClientId)
            {
                //cameras[i].SetActive(false);
            } else
            {
                camera = cameras[i];
            }
        }*/
        //camera = Instantiate(cameraPrefab);
        
            if (IsOwner)
            {
                camera.SetActive(true);
            }
        camera.transform.localPosition += new Vector3(0,0,0);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        if (IsServer || IsHost)
        {
            Transform spawnedObjectTransform = Instantiate(spawnerPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }
        isOnCooldown = false;
        cooldownRunning = false;
        health = 100;
        shootCooldown = 1500;
    }
    // Update is called once per frame
    void Update()
    {

        //Debug.Log(OwnerClientId + "; " + randomNumber.Value);

        
            if (!IsOwner) return;
        

        if (Input.GetKeyDown(KeyCode.T))
            {
                randomNumber.Value = Random.Range(0, 10);
            }


            /*Vector3 moveDir = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
            if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
            if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
            if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

            float moveSpeed = 3f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;*/
                //vector = Quaternion.AngleAxis(-45, Vector3.up) * vector;
                Vector3 sideMovement = transform.forward;
        sideMovement = Quaternion.AngleAxis(-90, Vector3.up) * sideMovement;
        float moveSpeed = 3f;
        
        if (Input.GetKey(KeyCode.W)) rb.velocity = transform.forward * moveSpeed;
        if (Input.GetKey(KeyCode.S)) rb.velocity = transform.forward * -moveSpeed;
        if (Input.GetKey(KeyCode.A)) rb.velocity = sideMovement * moveSpeed;
        if (Input.GetKey(KeyCode.D)) rb.velocity = sideMovement * -moveSpeed;

        camera.transform.position = this.transform.position + cameraOffset;
        rotation.y += Input.GetAxis("Mouse X");
        transform.eulerAngles = (Vector2)rotation * lookSpeed;
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
        cameraRotation.y = rotation.y;
        camera.transform.eulerAngles = (Vector2)cameraRotation * lookSpeed;

        if (Input.GetMouseButton(0))
        {
            Transform spawnedObjectTransform = Instantiate(bulletPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            isOnCooldown = true;
            onCooldown();
        }
    }
    async void onCooldown()
    {
        if (cooldownRunning) return; cooldownRunning = true;
        await Task.Delay((int)shootCooldown);
        cooldownRunning = false;
        isOnCooldown = false;
    }
}
