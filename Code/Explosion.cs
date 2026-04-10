using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Explosion : NetworkBehaviour
{
    private float explosionTime = 0.5f;
    private bool explosionActive = true;

private void OnTriggerEnter2D(Collider2D other) {
    if((other.gameObject.tag =="P1" || other.gameObject.tag =="P2" || other.gameObject.tag == "P3" || other.gameObject.tag =="P4") && explosionActive)
        {
            other.gameObject.GetComponent<Player>().Die();
    }
    if(other.gameObject.tag =="Enemy" && explosionActive){
        other.gameObject.GetComponent<Enemy>().Die();
    }
}
     
    void Update()
    {
        explosionTime -= Time.deltaTime;
        if(explosionTime <= 0){
            explosionActive = false;
        }
        if(transform.childCount==0){
            if(MainMenu.online && IsServer){
                gameObject.GetComponent<NetworkObject>().Despawn(true);
            }
            else if(!MainMenu.online){
                Destroy(gameObject);    
            }
        }
    }

}
