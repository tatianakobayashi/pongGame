using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    private bool followBall;

    private GameObject ball;

    private bool horizontal;

    // TODO - controles

    // Start is called before the first frame update
    void Start()
    {
        followBall = false;

        horizontal = gameObject.transform.position.x == 0;
        Debug.Log(horizontal);
    }

    // Update is called once per frame
    void Update()
    {
        if (followBall)
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
    }

    private void getBall()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    [PunRPC]
    public void Lost()
    {
        followBall = true;
    }
}
