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
    [SerializeField] public bool death = false;
    [SerializeField] bool isFirst = true;
    [SerializeField] PhysicsMaterial2D Bound1;
    [SerializeField] PhysicsMaterial2D Bound2;
    [SerializeField] GameObject[] walls;
    [SerializeField] public bool HIt = false;
    Rigidbody2D Ball;
    Rigidbody2D Shield;
    Collider2D ballCollider;
    Collider2D pSoul;
    Collider2D eSoul;

    /// <summary>
    /// 初期化処理。SetActiveがTrueになるたび呼び出される
    /// </summary>
    private void OnEnable()
    {
        isFirst = true;
        //死亡判定のリセット
        death = false;
        //Hit判定の強制初期化
        HIt = false;
        Bound1 = ballCollider.sharedMaterial;
        Bound1.bounciness = 1.0f;

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
        //失敗したかどうか判定
        if (ballCollider.IsTouching(eSoul))
        {
            HIt = true;
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
    public void ballRandomStart()
    {
        if (isFirst)
        {
            isFirst = false;  // 一回はすぎた
            Vector2 force = new Vector2(25000.0f,Random.Range(-600f,6000f));  // 力を設定
            Ball.AddForce(force);// 力を加える
        }

        //if (Ball.velocity.magnitude <= 1000 && Ball.velocity.magnitude >= 400)
        //{
            //speedX = Ball.velocity.x;
            //speedY = Ball.velocity.y;
            //moveDir = new Vector2(speedX, speedY);
        //}

        if(Ball.velocity.magnitude >100)
        {
            //Collision();
            //Ball.velocity = Vector2.zero; // 力を加える
            //Ball.velocity = new Vector2(moveDir.x,moveDir.y);
            //foreach ();
            Bound1.bounciness = 0f;
        }

        Debug.Log(Ball.velocity.magnitude);
    }
}
