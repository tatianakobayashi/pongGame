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
    public GameObject loginPanel, findMatchPanel, roomFoundPanel;

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
        roomFoundPanel.SetActive(true);
        Debug.Log("Joined Room");
        Debug.Log("Room name: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        roomFoundText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + "\nPlayer count: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }
}