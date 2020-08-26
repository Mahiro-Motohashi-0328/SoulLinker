using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// プレイヤーや敵など、ユニットのベースとなるコンポーネント
/// </summary>
public class Unit : MonoBehaviour
{
    [SerializeField] private int characterNomber = 1;
    [SerializeField] protected string m_name = "name";
    [SerializeField] protected int m_maxHp = 100;
    protected int m_currentHp;
    [SerializeField] protected int m_maxMp = 50;
    protected int m_currentMp;
    /// <summary>通常攻撃で与えるダメージ量</summary>
    [SerializeField] protected int m_attackPower = 5;
    /// <summary>魔法で回復するHPの量</summary>
    [SerializeField] protected int m_healPower = 25;
    /// <summary>回復魔法で消費するMP</summary>
    [SerializeField] protected int m_consumedMpForHeal = 10;
    /// <summary>攻撃魔法で与えるダメージ量</summary>
    [SerializeField] protected int m_magicPower = 25;
    /// <summary>攻撃魔法で消費するMP</summary>
    [SerializeField] protected int m_consumedMpForMagic = 20;
    /// <summary>ユニットの状態（防御など）</summary>
    protected UnitState m_status = UnitState.None;
    /// <summary>ユニットの画像（スプライト）</summary>
    [SerializeField] protected Sprite m_sprite;
    TextAsset csvFile; // CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    void Awake()
    {
        csvFile = Resources.Load("SoulLinkerキャラシート") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            string[] values = line.Split(',');
            csvDatas.Add(values); // , 区切りでリストに追加
        }

        // csvDatas[行][列]を指定して値を自由に取り出せる
        Debug.Log(csvDatas[characterNomber][0]);
        reader.Close();
        m_name = csvDatas[characterNomber][1];
        m_maxHp = int.Parse(csvDatas[characterNomber][2]);
        m_maxMp = int.Parse(csvDatas[characterNomber][3]);
        m_attackPower = int.Parse(csvDatas[characterNomber][4]);
        m_healPower = int.Parse(csvDatas[characterNomber][5]);
        m_consumedMpForMagic = int.Parse(csvDatas[characterNomber][6]);
        // インスタンス生成時はHP/MP共に最大にする
        m_currentHp = m_maxHp;
        m_currentMp = m_maxMp;
    }
    /// <summary>
    /// ユニット名
    /// </summary>
    public string Name
    {
        get { return m_name; }
    } 

    /// <summary>
    /// 最大HP
    /// </summary>
    public int MaxHp
    {
        get { return m_maxHp; }
    }

    /// <summary>
    /// 現在のHP
    /// </summary>
    public int Hp
    {
        get { return m_currentHp; }
    }

    /// <summary>
    /// 最大MP
    /// </summary>
    public int MaxMp
    {
        get { return m_maxMp; }
    }

    /// <summary>
    /// 現在のMP
    /// </summary>
    public int Mp
    {
        get { return m_currentMp; }
    }

    /// <summary>
    /// 通常攻撃の攻撃力
    /// </summary>
    public int AttackPower
    {
        get { return m_attackPower; }
    }

    public int HealPower
    {
        get { return m_healPower; }
    }

    public int MagicPower
    {
        get { return m_consumedMpForMagic; }
    }

    /// <summary>
    /// キャラクターのスプライト
    /// </summary>
    public Sprite Sprite
    {
        get { return m_sprite; }
    }
    /// <summary>
    /// ターン開始時に呼ぶ
    /// </summary>
    public void BeginTurn()
    {
        // ステータスをリセットする
        m_status = UnitState.None;
    }

    /// <summary>
    /// ダメージを食らう時に呼ぶ
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        // 防御中ならばダメージは半減する
        if (m_status == UnitState.Guard)
        {
            damage = damage / 2;
        }

        // ダメージの分だけHPを減らす。0より小さくならないようにする
        m_currentHp -= damage;
        m_currentHp = Mathf.Max(0, m_currentHp);
    }

    /// <summary>
    /// 回復魔法使用時に呼ぶ
    /// </summary>
    public void Heal()
    {
        // MPが足りていれば回復を発動する
        if (m_currentMp >= m_consumedMpForHeal)
        {
            m_currentMp -= m_consumedMpForHeal;
            m_currentHp += m_healPower;
            // 最大HPを超えないようにする
            m_currentHp = Mathf.Min(m_maxHp, m_currentHp);
        }
        // MPが足りていない時はただターンを消費してしまう
    }

    /// <summary>
    /// 防御をする時に呼ぶ
    /// </summary>
    public void Guard()
    {
        m_status = UnitState.Guard;
    }

    /// <summary>
    /// 魔法を使う時に呼ぶ。MPが足りていない時はターンを消費してダメージは与えられない。
    /// </summary>
    /// <returns>魔法によるダメージ量</returns>
    public int Magic()
    {
        int power = 0;  // 戻り値

        // MP が足りていたら魔法によるダメージを返す
        if (m_currentMp >= m_consumedMpForMagic)
        {
            m_currentMp -= m_consumedMpForMagic;
            power = m_magicPower;
        }

        return power;
    }
}

public enum UnitState
{
    /// <summary>通常状態</summary>
    None,
    /// <summary>防御</summary>
    Guard,
}