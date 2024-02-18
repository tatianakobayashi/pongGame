using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Connect : MonoBehaviourPunCallbacks
{
    private TMP_InputField playerName, roomNameInput;
    private TMP_Text roomFoundText;
    public GameObject loginPanel, findMatchPanel, roomFoundPanel, gameStatusPanel, gameOverPanel;

    public Transform[] startingPositions;

    private string[] playerTypes = { "paddleBottom", "paddleTop", "paddleRight", "paddleLeft" };

    public TMP_Text[] playerNames;

    // Start is called before the first frame update
    void Start()
    {
        // Login();
        loginPanel.SetActive(true);
        findMatchPanel.SetActive(false);
        roomFoundPanel.SetActive(false);

        playerName = loginPanel.GetComponentInChildren<TMP_InputField>();
        roomNameInput = findMatchPanel.GetComponentInChildren<TMP_InputField>();
        roomFoundText = roomFoundPanel.GetComponentInChildren <TMP_Text>();

        ClearNames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // -----------------------------------------------------
    public void Login()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("sa");


        PhotonNetwork.NickName = (playerName.text != null && playerName.text.Length > 0) ? playerName.text : "player" + Random.Range(0, 1000);

        Debug.Log("Player: " + PhotonNetwork.NickName);
        loginPanel.SetActive(false);
    }

    public void ButtonFindMatch()
    {

        PhotonNetwork.JoinLobby();
    }

    public void ButtonCreateRoom()
    {
        string roomName = (roomNameInput != null && roomNameInput.text.Length > 0) ? roomNameInput.text : "room" + Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 4};

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        findMatchPanel.SetActive(false);
    }

    public void ButtonExitMatch()
    {
        gameOverPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        ClearNames();
        findMatchPanel.SetActive(true);
    }

    private void ClearNames()
    {
        foreach(TMP_Text t in playerNames)
            t.text = "";
    }

    public void doExitGame()
    {
        Application.Quit();
    }

    // -----------------------------------------------------

    public override void OnConnected()
    {
        Debug.Log("Connected");
        Debug.Log($"Server: {PhotonNetwork.CloudRegion} Ping: {PhotonNetwork.GetPing()}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected on Master");

        //ButtonFindMatch();

        findMatchPanel.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed");

        string roomName = "room" + Random.Range(0, 1000);
        PhotonNetwork.CreateRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        //roomFoundPanel.SetActive(true);
        Debug.Log("Joined Room");
        Debug.Log("Room name: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        roomFoundText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + "\nPlayer count: " + PhotonNetwork.CurrentRoom.PlayerCount +
            "\nPlayer: " + PhotonNetwork.LocalPlayer.ActorNumber + "\nPaddle: " + playerTypes[PhotonNetwork.LocalPlayer.ActorNumber - 1];

        // Instancia o fundo e as paredes
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("Background", new Vector3(0, 0, 0), Quaternion.identity);
            PhotonNetwork.Instantiate("walls", new Vector3(0, -4.8f, 0), Quaternion.identity);
        }

        int pos = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        playerNames[pos].text = PhotonNetwork.LocalPlayer.NickName;

        PhotonNetwork.Instantiate(
            playerTypes[pos] + " Variant", // Nome do prefab
            startingPositions[pos].position,  // posição
            (startingPositions[pos].position.x == 0) ? Quaternion.identity : Quaternion.Euler(0, 0, 90) // rotação
            );

        // Instancia o game controller quando a sala tiver 4 jogadores
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("Ball", new Vector3(0, 0, 0), Quaternion.identity);
            PhotonNetwork.Instantiate("GameController", new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
