using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class SpawnerScript : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;

    private bool canSpawn;
    private bool onCooldown;
    private GameObject player;
    private Vector3 spawnLocation;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        canSpawn = false;
        player = GameObject.FindWithTag(tag: "Player");
        onCooldown = false;
        canSpawnEnemies();
    }
    private void Update()
    {
        if (canSpawn)
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab, spawnLocation, Quaternion.Euler(0,0,0));
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            canSpawn = false;

            spawnLocation = new Vector3(Random.RandomRange(-25, 25), 0, Random.RandomRange(-25, 25));
            float distance = Mathf.Sqrt((spawnLocation.x - player.transform.position.x) * (spawnLocation.x - player.transform.position.x) + (spawnLocation.y - player.transform.position.y) * (spawnLocation.y - player.transform.position.y));
            Debug.Log(distance);
            while (distance < 10)
            {
                spawnLocation = new Vector3(Random.RandomRange(-25, 25), 0, Random.RandomRange(-25, 25));
                distance = Mathf.Sqrt((spawnLocation.x - player.transform.position.x) * (spawnLocation.x - player.transform.position.x) + (spawnLocation.y - player.transform.position.y) * (spawnLocation.y - player.transform.position.y));
                Debug.Log(distance);
            }

            canSpawnEnemies();
        }
    }

    async void canSpawnEnemies()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            await Task.Delay(2000);
            onCooldown = false;
            canSpawn = true;
        }
    }
}
