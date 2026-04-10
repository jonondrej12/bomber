using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class destructibleWall : NetworkBehaviour
{
    private Animator an;
    private float destroyTime = 3f;
    private bool hasBeenDestroyed = false;
    public GameObject flamePowerup;
    public GameObject moreBombsPowerup;
    private int maxPowerupChance;
    public GameObject portal;
    GameObject spawnedPowerup;

    void Start()
    {
        an = GetComponent<Animator>();
        if (MainMenu.localMulti)
        {
            maxPowerupChance = 8;
        }
        else if (MainMenu.online)
        {
            maxPowerupChance = 6;
        }
    }

    void Update()
    {
        if (an.GetBool("isDestroyed") == true)
        {
            destroyTime -= Time.deltaTime;
            if (destroyTime <= 0)
            {
                if (MainMenu.online && IsServer)
                {
                    removeWallServerRpc();
                }
                else if (!MainMenu.online)
                {
                    Destroy(gameObject);
                }
            }
            if (!hasBeenDestroyed)
            {
                if (MainMenu.localMulti || MainMenu.online)
                {
                    hasBeenDestroyed = true;
                    int chanceToGetPowerup = Random.Range(1, maxPowerupChance);
                    if (chanceToGetPowerup == 3)
                    {
                        int whichPowerup = Random.Range(1, 3);
                        if (whichPowerup == 1)
                        {
                           // spawnPowerup(1);
                        }
                        else
                        {
                          //  spawnPowerup(2);
                        }
                    }
                }
                else{
                    hasBeenDestroyed = true;
                    GameObject[] walls = GameObject.FindGameObjectsWithTag("weakWall");
                    //šance na portál je jedna ku poètu zbývajících zdí
                    int portalChance = Random.Range(1, walls.Length + 1);
                    if(portalChance == 1 && !LevelGenerator.portalSpawned){
                        LevelGenerator.portalSpawned = true;
                        Instantiate(portal, transform.position, Quaternion.identity);
                    }
                    else if(portalChance == 2 && !LevelGenerator.powerupSpawned){
                        LevelGenerator.powerupSpawned = true;
                        int whichPowerup = Random.Range(1, 3);
                        if(whichPowerup == 1){
                            Instantiate(flamePowerup, transform.position, Quaternion.identity);
                        }
                        else{
                            Instantiate(moreBombsPowerup, transform.position, Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

    void spawnPowerup(int powerupNuber)
    {
        if (powerupNuber == 1)
        {
            spawnedPowerup = Instantiate(flamePowerup, transform.position, Quaternion.identity);
        }
        else
        {
            spawnedPowerup = Instantiate(moreBombsPowerup, transform.position, Quaternion.identity);
        }
        if (MainMenu.online && IsServer) 
        {
            createPowerupServerRpc();
        }
    }

    [ServerRpc]
    void removeWallServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn(true);
    }

    [ServerRpc]
    void createPowerupServerRpc()
    {
        spawnedPowerup.GetComponent<NetworkObject>().Spawn();
    }
}
