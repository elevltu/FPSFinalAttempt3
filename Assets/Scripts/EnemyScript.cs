using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class EnemyScript : NetworkBehaviour
{
    private float health;
    private bool canBeHurt;
    private bool isRunning;
    private GameObject player;

    private float playerX;
    private float playerZ;
    private Vector3 movementDirection;
    private Rigidbody rb;
    private float speed = 1;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        health = 100;
        canBeHurt = true;
        isRunning = false;
        player = GameObject.FindWithTag(tag: "Player");
        
        
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Destroy(gameObject);
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (player == null) return;
        playerX = player.transform.position.x;
        playerZ = player.transform.position.z;
        movementDirection = new Vector3(playerX - rb.position.x, 0, playerZ - rb.position.z);
        movementDirection.Normalize();
        movementDirection *= (speed * Time.deltaTime);
        rb.position += movementDirection;

        transform.LookAt(player.transform.position);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);


    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.tag.Equals("Bullet") && canBeHurt)
        {
            health -= other.gameObject.GetComponent<BulletScript>().damage;
            Destroy(other.gameObject);
            canBeHurt = false;
            invincibilityFrames();
        }
    }
    private async void invincibilityFrames()
    {
        if (!isRunning)
        {
            isRunning = true;
            await Task.Delay(20);
            isRunning= false;
            canBeHurt= true;
        }
    }
    /*private void OnCollisionEnter(Collision collider)
    {
        Debug.Log(collider.collider.gameObject);
        if (collider.collider.tag.Equals("Bullet"))
        {
            health -= collider.collider.gameObject.GetComponent<BulletScript>().damage;
            Destroy(collider.collider.gameObject);
            Debug.Log("HELP");
        }
    }*/
}
