using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Powerup : NetworkBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4")
        {
            if (MainMenu.online)
            {
                despawnPowerupServerRpc();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void despawnPowerupServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn(true);
    }

}
