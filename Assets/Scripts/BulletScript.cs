using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using System.Threading.Tasks;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    private Rigidbody rb;
    public float damage;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb = GetComponent<Rigidbody>();
        rb.position = new Vector3(rb.position.x, 1, rb.position.z);
        //rb.velocity = rb.gameObject.transform.forward * 40;
        DestroyAfterTime();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    async void DestroyAfterTime()
    {
        await Task.Delay(5000);
        if (rb != null)
        {
            Destroy(gameObject);
        }
    }
    public void setDamage(float a)
    {
        damage = a;
    }
}
