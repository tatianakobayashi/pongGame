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

    private int activePlayers;

    private List<string> names;

    // Start is called before the first frame update
    void Awake()
    {
        names = new List<string>();

        positions.Add("bottom", 1);
        positions.Add("top", 2);
        positions.Add("right", 3);
        positions.Add("left", 4);

        photonView = PhotonView.Get(this);

        // busca objetos dos jogadores
        players = GameObject.FindGameObjectsWithTag("Player");

        // Busca objeto das paredes
        gameOverPanel = GameObject.FindGameObjectWithTag("GameOver");

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
        activePlayers = 4;
        if (gameOverPanel != null)
            gameOverPanel.active = false;

        // envia sinal de início de jogo
        foreach (GameObject player in players)
        {
            player.GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All);

            names.Add(player.name);
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
                    Debug.Log(player.name);
                    activePlayers--;
                    names.Remove(player.name);
                    playerPhoton.RPC("Lost", RpcTarget.All);
                    if (activePlayers == 1)
                        photonView.RPC("GameOver", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    private void GameOver()
    {
        gameOverPanel.active = true;
        gameOverPanel.GetComponentInChildren<TMP_Text>().text = "Vencedor: " + names[0];

        names.Remove(names[0]);

        // envia sinal de fim de jogo
        foreach (GameObject player in players)
        {
            player.GetComponent<PhotonView>().RPC("GameOver", RpcTarget.All);
        }

        ball.GetComponent<PhotonView>().RPC("GameOver", RpcTarget.All);
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
