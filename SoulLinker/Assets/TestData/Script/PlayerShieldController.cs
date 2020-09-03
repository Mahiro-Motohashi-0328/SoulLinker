using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldController : MonoBehaviour
{
    public int speed = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤー動作
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.position += Vector3.left * speed;
            body.velocity = Vector2.up * speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.position += Vector3.right * speed;
            body.velocity = Vector2.down * speed;
        }

    }
}
