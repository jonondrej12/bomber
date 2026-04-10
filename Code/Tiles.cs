using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    private int xTile;
    private int yTile;

    public Vector2 checkPlayerPosition(float playerPosX, float playerPosY){
       xTile = (int)(playerPosX / 1.92);
       yTile = (int)(playerPosY / 1.92);
       return new Vector2(xTile, yTile);
    }
}
