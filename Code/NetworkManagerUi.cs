using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;

public class NetworkManagerUi : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private GameObject multiplayerUI;
    [SerializeField] private GameObject PlayersConnectedPanel;
    [SerializeField] private Text PlayersConnectedText;
    [SerializeField] private GameObject multiplayerJoinedUI;
    [SerializeField] private GameObject succesfullyJoinedIcon;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private Text endGameText;
    [SerializeField] private GameObject transitionScreen;
    private int playerIndex = 0;
    public NetworkVariable<int> playersReady = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private bool readyButttonPressed = false;
    public NetworkVariable<int> playersNumber = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private Player[] playersConnected;
    public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);
    public NetworkVariable<int> playersAlive = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    [SerializeField] Text ipAddressHostText;
    [SerializeField] InputField ip;

    [SerializeField] string ipAddress;
    [SerializeField] UnityTransport transport;

    private void Start()
    {
        if (MainMenu.online) {
            transitionScreen.SetActive(false);
        }
        ipAddress = "0.0.0.0";
        SetIpAddress();
        ipAddressHostText.gameObject.SetActive(false);
    }

    public void HostGame() {
        NetworkManager.Singleton.StartHost();
        GetLocalIPAddress();
        multiplayerUI.SetActive(false);
        PlayersConnectedPanel.SetActive(true);
        multiplayerJoinedUI.SetActive(true);

    }

    public void JoinGame() {
        ipAddress = ip.text;
        SetIpAddress();
        NetworkManager.Singleton.StartClient();
        multiplayerUI.SetActive(false);
        PlayersConnectedPanel.SetActive(true);
        multiplayerJoinedUI.SetActive(true);
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
        PlayersConnectedPanel.SetActive(true);
    }

    public void SetIpAddress()
    {
        transport.ConnectionData.Address = ipAddress;
        Debug.Log("Starting ip: " + ip);
    }

    private void Update() {
        if (!gameStarted.Value) {
            if (MainMenu.online && IsServer)
            {
                playersNumber.Value = NetworkManager.Singleton.ConnectedClients.Count;
            }
            PlayersConnectedText.text = "Players connected: " + playersNumber.Value.ToString() + "/4";
        }
        else if (gameStarted.Value && multiplayerJoinedUI.activeSelf && PlayersConnectedPanel.activeSelf)
        {
            multiplayerJoinedUI.SetActive(false);
            PlayersConnectedPanel.SetActive(false);
        }
        if (gameStarted.Value && IsServer)
        {
            lastPlayerCheckServerRpc();
        }
    }

    public void readyButton() {
        if (!readyButttonPressed) {
            readyButttonPressed = true;
            succesfullyJoinedIcon.SetActive(true);
            addPlayersReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void addPlayersReadyServerRpc()
    {
        playersReady.Value += 1;
        if (playersReady.Value == playersNumber.Value)
        {
            GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().wallMaxChance = 2;
            GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().createLayout();
            playersConnected = GameObject.FindObjectsOfType<Player>();
            foreach (Player player in playersConnected)
            {
                player.gameStarted.Value = true;
            }
            playersAlive.Value = playersNumber.Value;
            gameStarted.Value = true;
        }
    }

    [ServerRpc]
    public void lastPlayerCheckServerRpc() {
        if (playersAlive.Value <= 1)
        {
            playersConnected = GameObject.FindObjectsOfType<Player>();
            playersAlive.Value = playersNumber.Value;
            foreach (Player player in playersConnected)
            {
                if (player.aliveOnline.Value)
                {
                    EndGame(player.gameObject.tag);
                }
            }
        }
    }

    public void EndGame(string playerTag)
    {
        if (playerTag == "P1")
        {
            showEndGameUIClientRpc(1);
        }
        else if (playerTag == "P2")
        {
            showEndGameUIClientRpc(2);
        }
        else if (playerTag == "P3")
        {
            showEndGameUIClientRpc(3);
        }
        else if (playerTag == "P4")
        {
            showEndGameUIClientRpc(4);
        }
    }



    [ClientRpc]
    void showEndGameUIClientRpc(int whichPlayerWon)
    {
        endGameScreen.SetActive(true);
        if (whichPlayerWon == 1)
        {
            endGameText.text = ("RED WON!");
        }
        else if (whichPlayerWon == 2)
        {
            endGameText.text = ("YELLOW WON!");
        }
        else if (whichPlayerWon == 3)
        {
            endGameText.text = ("PINK WON!");
        }
        else if (whichPlayerWon == 4)
        {
            endGameText.text = ("BLUE WON!");
        }
    }

    public void exitButton()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MainMenu");
    }

    
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressHostText.gameObject.SetActive(true);
                ipAddressHostText.text = "IP for hosting: " + ip.ToString();
                ipAddress = ip.ToString();
                Debug.Log("local ip: " + ip);
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    
}
