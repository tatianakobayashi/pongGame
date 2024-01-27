using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Connect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Login():
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
        PhotonNetwork.NickName = "player";
    }

    public void ButtonFindMatch()
    {

        PhotonNetwork.JoinLobby();
    }

    public void ButtonCreateRoom()
    {
        string roomName = "temporary name";
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 4};

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
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

        ButtonFindMatch();
       
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
        Debug.Log("Joined Room");
        Debug.Log("Room name: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
}
