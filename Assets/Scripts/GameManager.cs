using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject Apple;
    public GameObject Banana;
    public GameObject Grape;
    public GameObject Suika;
    public GameObject Orange;
    public float fruitsInterval;

    // Use this for initialization
    void Start () {
        // 設定ファイルとプレハブの紐付け
        Hashtable fruits = new Hashtable{
            ["a"] = Apple,
            ["b"] = Banana,
            ["g"] = Grape,
            ["s"] = Suika,
            ["o"] = Orange,
        };

        // 設定ファイル読み込み
        TextAsset textFile = (TextAsset)Resources.Load("Conf/Stage");
        // 設定ファイルを配列変換
        string[] textData = textFile.text.Split('\n');
        int rowNum = textData[0].Split(',').Length;
        int columnNum = textData.Length;

        // 左上から右に生成していく
        for(int i = 0; i < rowNum; i++){
            string[] tempWords = textData[i].Split(',');
            for(int j = 0; j < columnNum; j++){
                if(tempWords[j] != null && fruits.ContainsKey(tempWords[j])){
                    // フルーツ生成
                    GameObject obj = (GameObject)fruits[tempWords[j]];
                    float x = j * fruitsInterval;
                    float y = 0.0f - i * fruitsInterval;
                    Instantiate (obj, new Vector3( x, y, 0.0f), Quaternion.identity);
                }else{
                    Debug.Log(tempWords[j]);
                }
            }
        }
    }
}
