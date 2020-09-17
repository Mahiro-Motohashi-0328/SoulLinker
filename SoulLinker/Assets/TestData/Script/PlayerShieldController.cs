using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldController : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] GameObject PlayerShield;
    [SerializeField] GameObject PlayerSoul;
    [SerializeField] GameObject enemySoul;
    [SerializeField] int speed = 5;
    [SerializeField] public int HitCount = 0;
    [SerializeField] public bool death = false;
    Rigidbody2D Ball;
    Rigidbody2D Shield;
    Rigidbody Player;
    Rigidbody enemy;
    Collider2D ballCollider;
    Collider2D pSoul;
    Collider2D eSoul;

    /// <summary>
    /// 初期化処理。SetActiveがTrueになるたび呼び出される
    /// </summary>
    private void OnEnable()
    {
        //死亡判定のリセット
        death = false;
        //Hit回数の初期化
        HitCount = 0;

    }

    // Start is called before the first frame update
    void Awake()
    {
        //アタックボールの制御
        Ball = ball.GetComponent<Rigidbody2D>();
        //プレイヤー動作
        Shield = PlayerShield.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<Collider2D>();
        pSoul = PlayerSoul.GetComponent<Collider2D>();
        eSoul = enemySoul.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballCollider.IsTouching(eSoul))
        {
            HitCount++;
        }

        if (ballCollider.IsTouching(pSoul))
        {
            death = true;
        }

    }
    /// <summary>
    /// プレイヤーの盾を動かす
    /// </summary>
    public void movementShield()
    {
        Shield.velocity = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.position += Vector3.left * speed;
            Shield.velocity = Vector2.up * speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.position += Vector3.right * speed;
            Shield.velocity = Vector2.down * speed;
        }
    }



}
