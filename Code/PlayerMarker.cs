using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("P1").GetComponent<Player>().gameStarted.Value)
        {
            Destroy(gameObject);
        }
    }
}
