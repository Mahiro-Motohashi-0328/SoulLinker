using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// バトルマネージャーのコンポーネント。シーンに一つしか存在してはいけない。
/// </summary>
public class TurnBasedBattleManager : MonoBehaviour
{
    /// <summary>バトルの状態</summary>
    BattleState m_battleState = BattleState.None;
    /// <summary>プレイヤー/敵の行うアクション</summary>
    ActionType m_action = ActionType.None;
    /// <summary>プレイヤーのオブジェクト</summary>
    Unit m_player;
    /// <summary>敵のオブジェクト</summary>
    Enemy m_enemy;
    /// <summary>ログ出力のための StringBuilder</summary>
    System.Text.StringBuilder m_builder;
    /// <summary>プレイヤーの画像</summary>
    [SerializeField] Image m_playerSprite;
    /// <summary>敵の画像</summary>
    [SerializeField] Image m_enemySprite;
    /// <summary>プレイヤーの名前</summary>
    [SerializeField] Text m_playerName;
    /// <summary>プレイヤーHPのゲージ</summary>
    [SerializeField] Slider m_playerHpGauge;
    /// <summary>プレイヤーMPのゲージ</summary>
    [SerializeField] Slider m_playerMpGauge;
    /// <summary>プレイヤーの名前</summary>
    [SerializeField] Text m_enemyName;
    /// <summary>敵HPのゲージ</summary>
    [SerializeField] Slider m_enemyHpGauge;
    /// <summary>敵MPのゲージ</summary>
    [SerializeField] Slider m_enemyMpGauge;
    /// <summary>戦況テキスト</summary>
    [SerializeField] public Text battleText;

    void Start()
    {
        m_builder = new System.Text.StringBuilder();
    }

    /// <summary>
    /// バトル開始時に呼ぶ。バトルの初期化をする。
    /// </summary>
    /// <param name="player">プレイヤーのオブジェクト</param>
    /// <param name="enemy">敵のオブジェクト</param>
    public void StartBattle(Unit player, Enemy enemy)
    {
        // バトル中ではない時はバトル開始。バトル中ならば警告を出す。
        if (m_battleState == BattleState.None)
        {
            m_battleState = BattleState.Start;
            m_action = ActionType.None;
            m_player = player;
            m_enemy = enemy;
            SetImages();
            RefreshGauges();
        }
        else
        {
            Debug.LogWarning("Can't start another battle during a battle.");
            Destroy(enemy.gameObject);
        }
    }

    void Update()
    {
        // 状態によって処理を分ける
        switch (m_battleState)
        {
            case BattleState.Start:
                m_battleState = BattleState.PlayerTurn;
                m_builder.AppendLine("Start Battle.");
                m_builder.AppendLine("BattleState: " + m_battleState.ToString());
                Refresh();
                break;
            case BattleState.PlayerTurn:
                // プレイヤーが行動を決めるまでは待機する
                if (m_action != ActionType.None)
                {
                    m_builder.AppendLine("BattleState: " + m_battleState.ToString());
                    m_player.BeginTurn();
                    DoAction(m_player, m_enemy);
                    Refresh();
                }
                break;
            case BattleState.EnemyTurn:
                m_builder.AppendLine("BattleState: " + m_battleState.ToString());
                m_enemy.BeginTurn();
                m_action = m_enemy.GetAction(); // 敵の行動を決める
                DoAction(m_enemy, m_player);
                Refresh();
                break;
            case BattleState.Won:
                m_builder.AppendLine("BattleState: " + m_battleState.ToString());
                m_battleState = BattleState.None;
                Refresh();
                Destroy(m_enemy.gameObject);    // 自分が勝ったら敵のオブジェクトは破棄する
                if (m_enemySprite)
                {
                    m_enemySprite.sprite = null;    // 敵の画像を消す
                }
                break;
            case BattleState.Lost:
                m_builder.AppendLine("BattleState: " + m_battleState.ToString());
                m_battleState = BattleState.None;
                Refresh();
                // 負けた時は特に何もしない
                if (m_playerSprite)
                {
                    m_playerSprite.sprite = null;   // プレイヤーの画像を消す
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// プレイヤーに対する行動の指示を与える
    /// </summary>
    /// <param name="action">行動の種類</param>
    public void SetPlayerAction(ActionType action)
    {
        m_action = action;
    }

    /// <summary>
    /// 行動する。行動内容は m_action によって変わる
    /// </summary>
    /// <param name="subject">行動の主体</param>
    /// <param name="target">攻撃対象</param>
    void DoAction(Unit subject, Unit target)
    {
        m_builder.AppendLine("Action: " + m_action.ToString());

        // 行動する
        switch (m_action)
        {
            case ActionType.Attack:
                target.Damage(subject.AttackPower);
                break;
            case ActionType.Heal:
                subject.Heal();
                break;
            case ActionType.Guard:
                subject.Guard();
                break;
            case ActionType.Magic:
                target.Damage(subject.Magic());
                break;
            default:
                break;
        }

        m_action = ActionType.None; // 行動が終わったら指示された行動内容を消す

        // 勝敗を判定し、状態を切り替える
        if (target.Equals(m_enemy))
        {
            if (target.Hp > 0)
            {
                m_battleState = BattleState.EnemyTurn;
            }
            else
            {
                m_battleState = BattleState.Won;
            }
        }
        else
        {
            if (target.Hp > 0)
            {
                m_battleState = BattleState.PlayerTurn;
            }
            else
            {
                m_battleState = BattleState.Lost;
            }
        }

        m_builder.AppendLine("Next BattleState: " + m_battleState.ToString());
    }

    /// <summary>
    /// StringBuilder の内容を出力し、消去する。現在の各種パラメーターを追加で出力する。
    /// </summary>
    void Refresh()
    {
        m_builder.AppendLine(m_player.Name + "'s HP: " + m_player.Hp);
        m_builder.AppendLine(m_player.Name + "'s MP: " + m_player.Mp);
        m_builder.AppendLine(m_enemy.Name + "'s HP: " + m_enemy.Hp);
        m_builder.AppendLine(m_enemy.Name + "'s MP: " + m_enemy.Mp);
        Debug.Log(m_builder.ToString());
        m_builder.Clear();
        RefreshGauges();
        battleText.text = m_player.Name + "'s HP: " + m_player.Hp;
    }

    /// <summary>
    /// プレイヤーと敵の画像をセットする
    /// </summary>
    void SetImages()
    {
        if (m_playerSprite)
        {
            m_playerSprite.sprite = m_player.Sprite;
        }

        if (m_playerName)
        {
            m_playerName.text = m_player.Name;
        }

        if (m_enemySprite)
        {
            m_enemySprite.sprite = m_enemy.Sprite;
        }

        if (m_enemyName)
        {
            m_enemyName.text = m_enemy.Name;
        }
    }

    /// <summary>
    /// HP/MP ゲージの表示を更新する
    /// </summary>
    void RefreshGauges()
    {
        if (m_playerHpGauge)
        {
            m_playerHpGauge.maxValue = m_player.MaxHp;
            m_playerHpGauge.value = m_player.Hp;
        }

        if (m_playerMpGauge)
        {
            m_playerMpGauge.maxValue = m_player.MaxMp;
            m_playerMpGauge.value = m_player.Mp;
        }

        if (m_enemyHpGauge)
        {
            m_enemyHpGauge.maxValue = m_enemy.MaxHp;
            m_enemyHpGauge.value = m_enemy.Hp;
        }

        if (m_enemyMpGauge)
        {
            m_enemyMpGauge.maxValue = m_enemy.MaxMp;
            m_enemyMpGauge.value = m_enemy.Mp;
        }
    }
}

/// <summary>
/// バトルの状態を表す
/// </summary>
public enum BattleState
{
    /// <summary>バトル中ではない</summary>
    None,
    /// <summary>バトル開始</summary>
    Start,
    /// <summary>プレイヤーのターン</summary>
    PlayerTurn,
    /// <summary>敵のターン</summary>
    EnemyTurn,
    /// <summary>プレイヤーが勝った</summary>
    Won,
    /// <summary>プレイヤーが負けた</summary>
    Lost,
}

/// <summary>
/// プレイヤー/敵の行動を表す
/// </summary>
public enum ActionType
{
    /// <summary>何もしていない</summary>
    None,
    /// <summary>通常攻撃</summary>
    Attack,
    /// <summary>回復魔法</summary>
    Heal,
    /// <summary>防御</summary>
    Guard,
    /// <summary>攻撃魔法</summary>
    Magic,
    /// <summary>何もしていない</summary>
    Idle,
}