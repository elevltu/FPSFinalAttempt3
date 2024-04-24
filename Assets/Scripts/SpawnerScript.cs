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
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        canSpawn = true;
        onCooldown = false;
    }
    private void Update()
    {
        if (canSpawn)
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            canSpawn = false;
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
