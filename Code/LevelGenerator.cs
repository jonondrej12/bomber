using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LevelGenerator : NetworkBehaviour
{
    private int baloonNumber = 3;
    public GameObject wall;
    public GameObject baloon;
    public GameObject fastEnemy;
    public GameObject multiplayerLayout;
    public static bool portalSpawned;
    public static bool powerupSpawned;
    public int wallMaxChance = 8;


    private void Start()
    {
        if (!MainMenu.localMulti && !MainMenu.online)
        {
            baloonNumber += variables.levelNumber/2;
            wallMaxChance -= variables.levelNumber/3;
            if(wallMaxChance < 4){
                wallMaxChance = 4;
            }
            SpawnMonsters(baloon, baloonNumber);
            createLayout();
            portalSpawned = false;
            powerupSpawned = false;
        }
        else if(MainMenu.localMulti){
            wallMaxChance = 3;
            createLayout();
        }

    }

    public void createLayout()
    {
        for (int row = 0; row < 9; row++)
        {
            if (row % 2 != 0)
            {
                for (int collum = 0; collum < 19; collum += 2)
                {
                    if (Random.Range(1, wallMaxChance) == 1)
                    {
                        if (row == 1)
                        {
                            if (collum != 0 && !MainMenu.online)
                            {
                                placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                            }
                            else if (collum != 0 && MainMenu.online && collum != 18)
                            {
                                placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));   
                            }
                        }
                        else if(row == 7){
                            if (MainMenu.online && collum != 0 && collum != 18)
                            {
                                placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                            }      
                            else if (MainMenu.localMulti && collum != 18)
                            {
                                placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                            }                        
                        }                        
                        else
                        {
                            placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                        }

                    }
                }
            }
            else
            {
                for (int collum = 0; collum < 19; collum++)
                {
                    if (Random.Range(1, wallMaxChance) == 1)
                    {
                        if (row == 0)
                        {
                            if(!MainMenu.online && collum != 0 && collum != 1){
                                placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                            }
                            else if(MainMenu.online && collum != 18 && collum != 17 && collum != 0 && collum != 1){
                                placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                            }           
                        }
                        else if(row == 8){
                            if (MainMenu.online && collum != 0 &&  collum != 1 && collum != 18 && collum != 17)
                            {
                                placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                            }    
                            else if (MainMenu.localMulti && collum != 18 && collum != 17)
                            {
                                placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                            }                                                      
                        }                        
                        else
                        {
                            placeWall(new Vector2((collum * 1.92f) + 0.96f, (row * -1.92f) - 0.96f));
                        }

                    }
                }
            }
        }

    }

    void SpawnMonsters(GameObject monster, int howManyMonsters)
    {
        int randomCollum = 0;
        int randomRow = 0;
        int monstersPlaced = 0;
        int iterationsN = 0;
        while (monstersPlaced < howManyMonsters && iterationsN < 100)
        {
            randomCollum = Random.Range(0, 19);
            randomRow = Random.Range(0, 9);
            iterationsN++;
            if (randomCollum + randomRow >= 4 && monster == baloon)
            {
                if(randomRow %2 == 0){
                    Instantiate(monster, new Vector2((randomCollum * 1.92f) + 0.96f, (randomRow * -1.92f) - 0.96f), Quaternion.identity);
                    monstersPlaced++;
                }
                else if(randomCollum %2 == 0){
                    Instantiate(monster, new Vector2((randomCollum * 1.92f) + 0.96f, (randomRow * -1.92f) - 0.96f), Quaternion.identity);
                    monstersPlaced++;
                }
            }
            else if(monster == fastEnemy){
                if(randomRow %2 == 0){
                    Instantiate(monster, new Vector2((randomCollum * 1.92f) + 0.96f, (randomRow * -1.92f) - 0.96f), Quaternion.identity);
                    monstersPlaced++;
                }
                else if(randomCollum %2 == 0){
                    Instantiate(monster, new Vector2((randomCollum * 1.92f) + 0.96f, (randomRow * -1.92f) - 0.96f), Quaternion.identity);
                    monstersPlaced++;
                }
            }
        }
    }

    public void TimeIsUp(){
        SpawnMonsters(fastEnemy, variables.levelNumber * 2);
    }

    void placeWall(Vector2 position){
        Collider2D collider = Physics2D.OverlapPoint(position);
        if (collider == null)
        {
            GameObject wallObject = Instantiate(wall, position, Quaternion.identity);
            if(MainMenu.online && IsServer){
                wallObject.GetComponent<NetworkObject>().Spawn();
            }
        }        
    }


}
