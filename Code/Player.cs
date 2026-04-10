using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    float movementSpeed = 1000;
    public Animator an;
    int maxBombs = 1;
    public int bombsPlaced = 0;
	public NetworkVariable<int> onlineBombsPlaced = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private Tiles tilesScript;
    private Vector2 tileIndex;
    public int bombPowerPlayer = 1;
    public GameObject bombPrefab;
    private bool alive = true;
    public NetworkVariable<bool> aliveOnline = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone);
    private float destroyTime = 2.5f;
    private bool gameOverUIShown = false;
    private GameObject bomb;
    private string bombTag;
    public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);
    public GameObject playerMarker;


    private void Start()
    {
        tilesScript = GameObject.FindGameObjectWithTag("GameTiles").GetComponent<Tiles>();
        if (!MainMenu.localMulti && !MainMenu.online)
        {
            maxBombs = variables.playerMaxBombCount;
            bombPowerPlayer = variables.playerBombPower;
        }
        if(MainMenu.online && IsOwner){
            Instantiate(playerMarker, new Vector2(transform.position.x, transform.position.y - 1.5f) , Quaternion.identity);
        }
    }

    void FixedUpdate()
    {
        if (!MainMenu.online)
        {
            if (alive)
            {
                Movement();
                BombPlacing();
            }
            else{
                destroyTime -= Time.deltaTime;
                if (!MainMenu.localMulti)
                {
                    if (destroyTime <= 0 && variables.lives > 0)
                    {
                        SceneManager.LoadScene("Game");
                        variables.lives -= 1;
                    }
                    else if (destroyTime <= 0)
                    {
                        GameObject.FindGameObjectWithTag("UI").GetComponent<UISetter>().GameOver();
                    }
                }
                else if (!gameOverUIShown)
                {
                    if (destroyTime <= 0)
                    {
                        if (gameObject.tag == "P1")
                        {
                            GameObject.FindGameObjectWithTag("UI").GetComponent<UISetter>().setScore(1, 2);
                        }
                        else
                        {
                            GameObject.FindGameObjectWithTag("UI").GetComponent<UISetter>().setScore(1, 1);
                        }
                        if (variables.playerOneScore == variables.winMatches || variables.playerTwoScore == variables.winMatches)
                        {
                            GameObject.FindGameObjectWithTag("UI").GetComponent<UISetter>().GameOver();
                            gameOverUIShown = true;
                        }
                        else
                        {
                            SceneManager.LoadScene("Game");
                        }
                    }
                }
            }
        }
        else if (IsOwner && gameStarted.Value && aliveOnline.Value) 
        {
            Movement();
            BombPlacing();
        }
    }

    void BombPlacing()
    {
        if (!MainMenu.online)
        {
            if (Input.GetKeyDown(KeyCode.Space) && bombsPlaced < maxBombs && gameObject.tag == "P1")
            {
                tileIndex = tilesScript.checkPlayerPosition(transform.position.x, transform.position.y);
                bomb = Instantiate(bombPrefab, new Vector2((tileIndex.x * 1.92f) + 0.96f, (tileIndex.y * 1.92f) - 1f), Quaternion.identity);
                bomb.GetComponent<Bomb>().bombPower = bombPowerPlayer;
                bomb.tag = "B1";
                bombsPlaced += 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightShift) && bombsPlaced < maxBombs && gameObject.tag == "P2")
            {
                tileIndex = tilesScript.checkPlayerPosition(transform.position.x, transform.position.y);
                GameObject bomb = Instantiate(bombPrefab, new Vector2((tileIndex.x * 1.92f) + 0.96f, (tileIndex.y * 1.92f) - 1f), Quaternion.identity);
                bomb.GetComponent<Bomb>().bombPower = bombPowerPlayer;
                bombsPlaced += 1;
                bomb.tag = "B2";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && onlineBombsPlaced.Value < maxBombs)
        {
            tileIndex = tilesScript.checkPlayerPosition(transform.position.x, transform.position.y);
            if(gameObject.tag == "P1"){
                bombTag = "B1";
            }
            else if(gameObject.tag == "P2"){
                bombTag = "B2";
            }
            else if (gameObject.tag == "P3")
            {
                bombTag = "B3";
            }
            else if (gameObject.tag == "P4")
            {
                bombTag = "B4";
            }
            SpawnBombServerRpc(tileIndex, bombPowerPlayer, bombTag, gameObject.tag);
        }
    }

    void Movement()
    {
        if (gameObject.tag == "P1" || MainMenu.online)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= new Vector3(0.0035f, 0, 0) * Time.deltaTime * movementSpeed;
                an.SetBool("IsRunning", true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(0.0035f, 0, 0) * Time.deltaTime * movementSpeed;
                an.SetBool("IsRunning", true);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, 0.0035f, 0) * Time.deltaTime * movementSpeed;
                an.SetBool("IsRunning", true);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position -= new Vector3(0, 0.0035f, 0) * Time.deltaTime * movementSpeed;
                an.SetBool("IsRunning", true);
            }
            else if (
                !Input.GetKey(KeyCode.A)
                && !Input.GetKey(KeyCode.D)
                && !Input.GetKey(KeyCode.W)
                && !Input.GetKey(KeyCode.S)
            )
            {
                an.SetBool("IsRunning", false);
            }
        }
        else if (gameObject.tag == "P2" && !MainMenu.online)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(0.0035f, 0, 0) * Time.deltaTime * movementSpeed;
                an.SetBool("IsRunning", true);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(0.0035f, 0, 0) * Time.deltaTime * movementSpeed;
                an.SetBool("IsRunning", true);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0, 0.0035f, 0) * Time.deltaTime * movementSpeed;
                an.SetBool("IsRunning", true);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position -= new Vector3(0, 0.0035f, 0) * Time.deltaTime * movementSpeed;
                an.SetBool("IsRunning", true);
            }
            else if (
                !Input.GetKey(KeyCode.DownArrow)
                && !Input.GetKey(KeyCode.UpArrow)
                && !Input.GetKey(KeyCode.LeftArrow)
                && !Input.GetKey(KeyCode.RightArrow)
            )
            {
                an.SetBool("IsRunning", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "flame")
        {
            bombPowerPlayer += 1;
            if(!MainMenu.online && !MainMenu.localMulti) {
                variables.playerBombPower += 1;
            }
        }
        else if (other.gameObject.tag == "moreBombs")
        {
            maxBombs += 1;
            if (!MainMenu.online && !MainMenu.localMulti)
            {
                variables.playerMaxBombCount += 1;
            }
        }
    }

    public void Die()
    {
        if (aliveOnline.Value || alive) 
        {
            an.SetBool("alive", false);
            alive = false;
            if (IsOwner && MainMenu.online) {
                if (gameObject.tag == "P1")
                {
                    playerDiedServerRpc(1);
                }
                else if (gameObject.tag == "P2")
                {
                    playerDiedServerRpc(2);
                }
                else if (gameObject.tag == "P3")
                {
                    playerDiedServerRpc(3);
                }
                else if (gameObject.tag == "P4")
                {
                    playerDiedServerRpc(4);
                }
            }

        }

    }

    [ServerRpc]
    void SpawnBombServerRpc(Vector2 position, int bombPow, string bTag, string playerTag){
        GameObject bomb = Instantiate(bombPrefab, new Vector2((position.x * 1.92f) + 0.96f, (position.y * 1.92f) - 1f), Quaternion.identity); 
        bomb.GetComponent<Bomb>().bombPower = bombPow;
        bomb.tag = bTag;
        GameObject.FindGameObjectWithTag(playerTag).GetComponent<Player>().onlineBombsPlaced.Value += 1;
        bomb.GetComponent<NetworkObject>().Spawn();
    }

    public override void OnNetworkSpawn()
    {
        GameObject.FindGameObjectWithTag("Spawner").GetComponent<PlayerSpawner>().SpawnPlayerMultiplayer(gameObject);
    }

    [ServerRpc]
    private void playerDiedServerRpc(int whoDied)
    {
        Debug.Log("PLAYERS: " + GameObject.FindGameObjectWithTag("NetworkUI").GetComponent<NetworkManagerUi>().playersAlive.Value);
        GameObject.FindGameObjectWithTag("NetworkUI").GetComponent<NetworkManagerUi>().playersAlive.Value -= 1;
        if (whoDied == 1)
        {
            GameObject.FindGameObjectWithTag("P1").GetComponent<Player>().aliveOnline.Value = false;
        }
        else if (whoDied == 2)
        {
            GameObject.FindGameObjectWithTag("P2").GetComponent<Player>().aliveOnline.Value = false;
        }
        else if(whoDied == 3)
        {
            GameObject.FindGameObjectWithTag("P3").GetComponent<Player>().aliveOnline.Value = false;
        }
        else if(whoDied == 4)
        {
            GameObject.FindGameObjectWithTag("P4").GetComponent<Player>().aliveOnline.Value = false;
        }

    }



}
