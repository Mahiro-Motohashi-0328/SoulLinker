using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトル開始やプレイヤーのコマンドをバトルマネージャーに伝えるコンポーネント。ボタンで操作するためにこのコンポーネントを経由する必要がある。
/// TurnBasedBattleManager と同じオブジェクトにアタッチすること。
/// </summary>
[RequireComponent(typeof(TurnBasedBattleManager))]
public class BattleController : MonoBehaviour
{
    /// <summary>プレイヤーのオブジェクト</summary>
    [SerializeField] Unit m_player;
    /// <summary>敵のプレハブ</summary>
    [SerializeField] Enemy m_enemyPrefab;
    /// <summary>敵のプレハブ（複数）</summary>
    [SerializeField] Enemy[] m_enemyPrefabsForRandomBattle;
    TurnBasedBattleManager m_battleManager;

    void Start()
    {
        m_battleManager = GetComponent<TurnBasedBattleManager>();
        // 敵のプレハブからオブジェクトを作り、バトルを開始する
        Enemy enemy = Instantiate(m_enemyPrefab) as Enemy;
        m_battleManager.StartBattle(m_player, enemy);
    }

    /// <summary>
    /// 設定した内容でバトルを開始する
    /// </summary>
    public void StartBattle()
    {
        // 敵のプレハブからオブジェクトを作り、バトルを開始する
        Enemy enemy = Instantiate(m_enemyPrefab) as Enemy;
        m_battleManager.StartBattle(m_player, enemy);
    }

    /// <summary>
    /// 設定した内容でバトルを開始する。複数設定した敵からランダムに一つ選んでバトルを開始する。
    /// </summary>
    public void StartRandomBattle()
    {
        // 設定した敵からランダムに一つ選ぶ
        int i = Random.Range(0, m_enemyPrefabsForRandomBattle.Length);
        Enemy enemyPrefab = m_enemyPrefabsForRandomBattle[i];
        Enemy enemy = Instantiate(enemyPrefab) as Enemy;
        m_battleManager.StartBattle(m_player, enemy);
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    public void PlayerAttack()
    {
        m_battleManager.SetPlayerAction(ActionType.Attack);
    }

    /// <summary>
    /// 防御
    /// </summary>
    public void PlayerGuard()
    {
        m_battleManager.SetPlayerAction(ActionType.Guard);
    }

    /// <summary>
    /// 攻撃魔法
    /// </summary>
    public void PlayerMagic()
    {
        m_battleManager.SetPlayerAction(ActionType.Magic);
    }

    /// <summary>
    /// 回復魔法
    /// </summary>
    public void PlayerHeal()
    {
        m_battleManager.SetPlayerAction(ActionType.Heal);
    }
}
