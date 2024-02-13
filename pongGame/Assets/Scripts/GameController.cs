using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviour
{

    private bool ongoing;
    private PhotonView photonView;

    private GameObject ball;

    private GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);

        // busca objetos dos jogadores
        players = GameObject.FindGameObjectsWithTag("Player");

        // Instancia bola
        ball = PhotonNetwork.Instantiate("Ball", new Vector3(0, 0, 0), Quaternion.identity);

        // envia rpc sinalizando que a sala está cheia
        foreach (GameObject player in players)
        {
            player.GetComponent<PhotonView>().RPC("FullRoom", RpcTarget.All);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [PunRPC]
    public void StartGame()
    {
        // envia sinal de início de jogo
        foreach (GameObject player in players)
        {
            player.GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All);
        }

        ball.GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All);
    }

        /*
        public void StartGame()
        {
            ongoing = true;
            photonView.RPC("StartGame", RpcTarget.All);
        }

        public void EndGame()
        {
            ongoing = false;
        }

        public bool Ongoing()
        {
            return ongoing;
        }
        */
    }
