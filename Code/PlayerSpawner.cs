using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;
    public GameObject spawnPoint4;
    public int spawnPointIndex = 1;
    public GameObject bomberman;


    void Start()
    {
        if(!MainMenu.online){
            GameObject bomber = Instantiate(bomberman, spawnPoint1.transform.position, Quaternion.identity);
            bomber.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            bomber.tag = "P1";
            if(MainMenu.localMulti){
                bomber = Instantiate(bomberman, spawnPoint4.transform.position, Quaternion.identity);
                bomber.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
                bomber.tag = "P2";
            }
        }
    }

    public void SpawnPlayerMultiplayer(GameObject player){
        if(spawnPointIndex == 1){
            player.transform.position = spawnPoint1.transform.position;
            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            player.tag = "P1";
        }
        else if(spawnPointIndex == 2){
            player.transform.position = spawnPoint2.transform.position;
            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
            player.tag = "P2";
        }
        else if(spawnPointIndex == 3){
            player.transform.position = spawnPoint3.transform.position;
            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.magenta;
            player.tag = "P3";
        }
        else if(spawnPointIndex == 4){
            player.transform.position = spawnPoint4.transform.position;
            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
            player.tag = "P4";
        }
        spawnPointIndex += 1;
    }

    public void RespawnPlayersMultiplayer(int playersNumber)
    {
        GameObject.FindGameObjectWithTag("P1").transform.position = spawnPoint1.transform.position;
        GameObject.FindGameObjectWithTag("P2").transform.position = spawnPoint2.transform.position;
        if(playersNumber >= 3) { 
            GameObject.FindGameObjectWithTag("P3").transform.position = spawnPoint3.transform.position;
        }
        if (playersNumber == 4)
        {
            GameObject.FindGameObjectWithTag("P4").transform.position = spawnPoint4.transform.position;
        }
    }


}
