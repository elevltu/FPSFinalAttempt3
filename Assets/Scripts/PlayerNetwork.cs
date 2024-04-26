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

    public GameObject endCanvas;
    public GameObject mainCanvas;

    public int health;
    private float shootCooldown;
    private bool isOnCooldown;
    private bool cooldownRunning;
    private Vector3 moveDirection = new Vector3 (0, 0, 0);
    private float damage;

    private bool isInvincible;
    private bool isOnInvincible;

    private Transform player;
    private Vector3 cameraOffset = new Vector3(0, 1, 0);

    public override void OnNetworkSpawn() {
        menuCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (menuCamera != null)
        {
            menuCamera.SetActive(false);
        }
        
        camera = GameObject.FindGameObjectWithTag("PlayerCamera");

        //endCanvas = GameObject.FindGameObjectWithTag("EndUI");
       // mainCanvas = GameObject.FindGameObjectWithTag("MainUI");
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
        if ((IsServer || IsHost) && GameObject.FindGameObjectWithTag("Spawner") == null)
        {
            Transform spawnedObjectTransform = Instantiate(spawnerPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }
        isOnCooldown = false;
        cooldownRunning = false;
        health = 100;
        shootCooldown = 1700;
        player = gameObject.GetComponent<Transform>();
        damage = 50;
        isInvincible = false; isOnInvincible = false;
        mainCanvas = Instantiate(mainCanvas);
        endCanvas = Instantiate(endCanvas);
        mainCanvas.SetActive(true);
        endCanvas.SetActive(false);
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

        Vector3 movementVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) movementVector += transform.forward * moveSpeed;
        if (Input.GetKey(KeyCode.S)) movementVector += transform.forward * -moveSpeed;
        if (Input.GetKey(KeyCode.A)) movementVector += sideMovement * moveSpeed;
        if (Input.GetKey(KeyCode.D)) movementVector += sideMovement * -moveSpeed;

        rb.velocity = movementVector;

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

        if (Input.GetMouseButton(0) && !isOnCooldown)
        {
            Transform spawnedObjectTransform = Instantiate(bulletPrefab, rb.position, Quaternion.Euler(new Vector3(rb.rotation.x, rb.rotation.y, rb.rotation.z)));
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            spawnedObjectTransform.GetComponent<BulletScript>().setDamage(damage);
            spawnedObjectTransform.GetComponent<Rigidbody>().velocity = rb.transform.forward * 20;
            isOnCooldown = true;
            onCooldown();
        }

        if (health <= 0)
        {
            camera.GetComponent<CameraScript>().playerDead = true;
            endCanvas.SetActive(true);
            mainCanvas.SetActive(false);
            endCanvas.GetComponent<EndCanvasScript>().onPlayerDie();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Destroy(player.gameObject);
        }
        else if (health <= 1)
        {
            damage = 1000;
            shootCooldown = 50;
        } else if (health <= 10)
        {
            damage = 200;
            shootCooldown = 200;
        } else if (health < 25)
        {
            damage = 100;
            shootCooldown = 500;
        } else if (health <= 50)
        {
            damage = 75;
            shootCooldown = 1000;
        }
    }
    async void onCooldown()
    {
        if (cooldownRunning) return; cooldownRunning = true;
        await Task.Delay((int)shootCooldown);
        cooldownRunning = false;
        isOnCooldown = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy" && !isInvincible)
        {
            health -= 10;
            isInvincible = true;
            invincibilityFrames();
        }
    }
    async void invincibilityFrames()
    {
        if (!isOnInvincible)
        {
            isOnInvincible = true;
            await Task.Delay(500);
            isOnInvincible = false;
            isInvincible = false;
        }
    }
}
