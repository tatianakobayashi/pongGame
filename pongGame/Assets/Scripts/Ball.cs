using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    private GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void StartGame()
    {
        Vector2 randomDirection = new Vector2(Random.value, Random.value).normalized;
        Debug.Log("random direction: " + randomDirection);
        rb.AddForce(randomDirection * -600f);
    }

    [PunRPC]
    public void ResetGame()
    {
        rb.AddForce(new Vector2(0, 0));
        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("hit wall");
            Debug.Log(collision.gameObject.name);
            gameController.GetComponent<PhotonView>().RPC("HitWall", RpcTarget.All, collision.gameObject);
        }
        // TODO - fim de jogo
    }
}
