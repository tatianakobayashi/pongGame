using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    private GameObject gameController;


    private float maxVelocity = 50f, sqrMaxVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sqrMaxVelocity = maxVelocity * maxVelocity;
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var v = rb.velocity;

        if (v.sqrMagnitude > sqrMaxVelocity)
        { 
            rb.velocity = v.normalized * maxVelocity;
        }
    }

    [PunRPC]
    public void StartGame()
    {
        Vector2 randomDirection = new Vector2(Random.value, Random.value).normalized;
        Debug.Log("random direction: " + randomDirection);
        rb.AddForce(randomDirection * -100f);
    }

    [PunRPC]
    public void ResetGame()
    {
        rb.AddForce(new Vector2(0, 0));
        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    [PunRPC]
    public void GameOver()
    {
        rb.AddForce(new Vector2(0, 0));
        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //Debug.Log("hit wall");
            //Debug.Log(collision.gameObject.name);
            gameController.GetComponent<PhotonView>().RPC("HitWall", RpcTarget.All, collision.gameObject.name);
        }
        // TODO - fim de jogo
    }
}
