using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Paddle : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private bool followBall;

    private GameObject ball;

    private bool horizontal;

    private Vector3 mouse;

    private bool ongoing, fullRoom;

    private Rigidbody2D rb;

    private GameObject gameController;

    // TODO - controles

    // Start is called before the first frame update
    void Awake()
    {
        followBall = false;
        ongoing = false;

        horizontal = gameObject.transform.position.x == 0;
        Debug.Log(horizontal);

        gameController = GameObject.FindGameObjectWithTag("GameController");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (gameController != null && photonView.IsMine && photonView.AmOwner && fullRoom)
        {
            // Inícia o jogo se a sala estiver cheia e for o dono da sala
            if (Input.GetKeyDown(KeyCode.Space))
                gameController.GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All);
        }
        else if (gameController == null)
        {
            getGameController();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followBall) // Segue a bola
        {
            if (ball != null)
            {
                if (horizontal)
                    gameObject.transform.position = new Vector3(ball.transform.position.x, gameObject.transform.position.y);
                else
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, ball.transform.position.y);
            }
            else
            {
                getBall();
            }

        }
        else if (photonView.IsMine && ongoing)
        {
            // Movimentação seguindo a posição do mouse
            Vector3 direction;
            mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition);
            if (horizontal)
                direction = new Vector3(mouse.x, 0);
            else
                direction = new Vector3(0, mouse.y);

            //Debug.Log("Mouse: " + mouse + " direction: " + direction);

            rb.AddForce(direction.normalized * 40f);
        } 
    }

    private void getBall()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    private void getGameController()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    [PunRPC]
    public void Lost()
    {
        StartCoroutine(WaitBallMove());
    }

    IEnumerator WaitBallMove()
    {
        yield return new WaitForSeconds(0.5f);
        followBall = true;
    }

    [PunRPC]
    public void StartGame()
    {
        ongoing = true;
    }

    [PunRPC]
    public void FullRoom()
    {
        fullRoom = true;
    }
}
