using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のベースとなるコンポーネント。行動パターンを実装している。
/// </summary>
public class Enemy : Unit
{
    /// <summary>攻撃をする確率</summary>
    [SerializeField, Range(0, 1)] float m_attackWeight = 0.5f;
    /// <summary>回復をする確率</summary>
    [SerializeField, Range(0, 1)] float m_healWeight = 0.1f;
    /// <summary>防御をする確率</summary>
    [SerializeField, Range(0, 1)] float m_guardWeight = 0.1f;
    /// <summary>攻撃魔法を使う確率</summary>
    [SerializeField, Range(0, 1)] float m_magicWeight = 0.2f;

    /// <summary>
    /// 行動を決める。この関数はかなりいい加減なので、設定値に注意すること。
    /// </summary>
    /// <returns>行動</returns>
    public ActionType GetAction()
    {
        float dice = Random.Range(0, 1f);   // サイコロを振る

        // サイコロの目によって行動を決めて返す
        if (dice < m_attackWeight)
        {
            return ActionType.Attack;
        }
        else if (m_attackWeight <= dice && dice < m_attackWeight + m_healWeight)
        {
            return ActionType.Heal;
        }
        else if (m_attackWeight + m_healWeight <= dice && dice < m_attackWeight + m_healWeight + m_guardWeight)
        {
            return ActionType.Guard;
        }
        else if (m_attackWeight + m_healWeight + m_guardWeight <= dice && dice < m_attackWeight + m_healWeight + m_guardWeight + m_magicWeight)
        {
            return ActionType.Magic;
        }

        return ActionType.Idle;
    }
}


