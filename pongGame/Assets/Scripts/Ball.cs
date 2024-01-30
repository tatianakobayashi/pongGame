using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void StartGame()
    {
        Vector2 randomDirection = new Vector2(Random.value, Random.value).normalized;
        GetComponent<Rigidbody2D>().AddForce(randomDirection * -600f);
    }
}
