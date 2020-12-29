using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class CSVReader : MonoBehaviour
{
    public void CSVRead(TextAsset csvFile, List<string[]> csvDatas)
    {
        //csvFile = Resources.Load("SoulLinkerキャラシート") as TextAsset; // Resouces下のCSV読み込み
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
        reader.Close();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
