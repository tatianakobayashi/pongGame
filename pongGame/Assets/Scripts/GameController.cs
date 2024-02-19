using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameController : MonoBehaviour
{

    private bool ongoing;
    private PhotonView photonView;

    private GameObject ball, gameOverPanel;

    private GameObject[] players;

    private Dictionary<string, int> positions = new Dictionary<string, int>();

    private Dictionary<int, string> playersById;

    private int activePlayers, winner;

    private List<string> names;

    private Connect connect;

    // Start is called before the first frame update
    void Start()
    {
        names = new List<string>();
        playersById = new Dictionary<int, string>();

        positions.Add("bottom", 1);
        positions.Add("top", 2);
        positions.Add("right", 3);
        positions.Add("left", 4);

        photonView = PhotonView.Get(this);

        // busca objetos dos jogadores
        players = GameObject.FindGameObjectsWithTag("Player");

        GetGameOverPanel();

        connect = GameObject.FindGameObjectWithTag("Connect").GetComponent<Connect>();

        // Instancia bola
        ball = GameObject.FindGameObjectWithTag("Ball");

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
        Debug.Log("StartGame");

        activePlayers = 4;
        /*
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        else
            GetGameOverPanel();
        */
        connect.StartGame();

        // envia sinal de início de jogo
        foreach (GameObject player in players)
        {
            //names.Add(player.name);

            Debug.Log("Nome objeto: " + player.name + " nickname: " + player.GetComponent<PhotonView>().name);

            player.GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All);
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            names.Add(p.NickName);
            playersById.Add(p.ActorNumber, p.NickName);
        }

        ball.GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All);
    }


    [PunRPC]
    public void HitWall(string wallName)
    {
        // TODO
        int position;

        if (positions.TryGetValue(wallName.Replace("wall", "").ToLower(), out position))
        {

            foreach (GameObject player in players)
            {
                PhotonView playerPhoton = player.GetComponent<PhotonView>();

                if (playerPhoton.CreatorActorNr == position)
                {
                    activePlayers--;
                    string loser;
                    playersById.TryGetValue(playerPhoton.CreatorActorNr, out loser);
                    names.Remove(loser);
                    playerPhoton.RPC("Lost", RpcTarget.All);
                    if (activePlayers == 1)
                    {
                        photonView.RPC("GameOver", RpcTarget.All);
                    }

                }
            }
        }
    }

    [PunRPC]
    private void GameOver()
    {
        /*
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.GetComponentInChildren<TMP_Text>().text = "Vencedor: " + names[0];
        }
        else
        {
            GetGameOverPanel();
        }
        */

        connect.GameOver(names[0]);

        names.Remove(names[0]);

        // envia sinal de fim de jogo
        foreach (GameObject player in players)
        {
            player.GetComponent<PhotonView>().RPC("GameOver", RpcTarget.All);
        }

        ball.GetComponent<PhotonView>().RPC("GameOver", RpcTarget.All);
    }

    private void GetGameOverPanel()
    {
        // Busca objeto das paredes
        gameOverPanel = GameObject.FindGameObjectWithTag("GameOver");
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
