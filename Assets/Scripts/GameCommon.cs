using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCommon : MonoBehaviour
{
    public int stageAllNum;
    public List<Sprite> fruitsSpriteList;

    private List<List<int>> fruitsTypeList = new List<List<int>>();
    private List<(int[,], int[,])> fruitsMapList = new List<(int[,], int[,])>();
    private List<int> rowNumList = new List<int>();
    private List<int> columnNumList = new List<int>();
    private Dictionary<int, Dictionary<string, bool>> stageDirectionConditionList = new Dictionary<int, Dictionary<string, bool>>(){
        {1, new Dictionary<string, bool>(){{"vertical", true}, {"horizontal", true}, {"diagonal", true}}},
        {2, new Dictionary<string, bool>(){{"vertical", true}, {"horizontal", true}, {"diagonal", true}}},
        {3, new Dictionary<string, bool>(){{"vertical", true}, {"horizontal", true}, {"diagonal", true}}},
    };

    private readonly string initMapPath = "InitMap";
    private readonly string compMapPath = "CompMap";

    void Start(){
        DontDestroyOnLoad(this);
        this.fruitsTypeList.Add(new List<int>());
        this.stageAllNum = this.getStageAllNum();
        for(int i = 0; i < this.stageAllNum; i++){
            this.fruitsMapList.Add(this.getFruitsMaps(i + 1));
            this.fruitsTypeList[i].Sort();
        }
    }

    // フルーツ画像取得
    public Sprite getFrutisSprite(int type){
        return this.fruitsSpriteList[type];
    }

    // 行数取得
    public int getRowNum(int stageNum){
        return this.rowNumList[stageNum - 1];
    }

    // 列数取得
    public int getColumnNum(int stageNum){
        return this.columnNumList[stageNum - 1];
    }

    // マス数取得
    public int getSquareNum(int stageNum){
        return this.rowNumList[stageNum - 1];
    }

    // フルーツ種類取得
    public List<int> getFrutisTypeList(int stageNum){
        return this.fruitsTypeList[stageNum - 1];
    }

    // フルーツ種類数取得
    public int getFruitsTypeNum(int stageNum){
        return this.fruitsTypeList[stageNum - 1].Count;
    }

    // フルーツ初期配置取得
    public int[,] getFruitsInitMap(int stageNum){
        (int[,] init, int[,] comp) fruitsMap = this.fruitsMapList[stageNum - 1];
        return fruitsMap.init;
    }

    // フルーツ完成配置取得
    public int[,] getFruitsCompMap(int stageNum){
        (int[,] init, int[,] comp) fruitsMap = this.fruitsMapList[stageNum - 1];
        return fruitsMap.comp;
    }

    // 
    public Dictionary<string, bool> getDirectionConditionList(int stageNum){
        Dictionary<string, bool> directionConditionList = new Dictionary<string, bool>();
        if(this.stageDirectionConditionList.ContainsKey(stageNum)){
            directionConditionList = this.stageDirectionConditionList[stageNum];
        }else{
            directionConditionList = new Dictionary<string, bool>(){{"vertical", true}, {"horizontal", true}, {"diagonal", true}};
        }
        return directionConditionList;
    }

    // 全ステージ数取得
    private int getStageAllNum(){
        string path = Application.dataPath + "/Resources/Conf/";
        string[] initMapFiles = Directory.GetFiles(path + this.initMapPath, "*.txt", SearchOption.AllDirectories);
        string[] compMapFiles = Directory.GetFiles(path + this.compMapPath, "*.txt", SearchOption.AllDirectories);
        return Mathf.Min(initMapFiles.Length, compMapFiles.Length);
    }

    // フルーツ初期、完成配置取得
    private (int[,], int[,]) getFruitsMaps(int stageNum){
        int initMapRowNum = 0, initMapColumnNum = 0;
        int compMapRowNum = 0, compMapColumnNum = 0;
        int[,] initMap = this.getFruitsMap(ref initMapRowNum, ref initMapColumnNum, stageNum, this.initMapPath);
        int[,] compMap = this.getFruitsMap(ref compMapRowNum, ref compMapColumnNum, stageNum, this.compMapPath);
        if(initMapRowNum != compMapRowNum || initMapColumnNum != compMapColumnNum){
            Debug.Log("エラー：初期配置と完成配置の大きさが一致しません（stage" + stageNum + "）");
        }
        this.rowNumList.Add(initMapRowNum);
        this.columnNumList.Add(initMapColumnNum);
        return (initMap, compMap);
    }

    // フルーツ配置取得
    private int[,] getFruitsMap(ref int rowNum, ref int columnNum, int stageNum, string path){
        string txtPath = "Conf/" + path + "/Stage" + stageNum;
        TextAsset textFile = (TextAsset)Resources.Load(txtPath);
        string[] textData = textFile.text.Split('\n');
        rowNum = textData[0].Split(',').Length;
        columnNum = textData.Length;
        if(rowNum != columnNum){
            Debug.Log("エラー：縦と横の数が一致しません");
        }
        // 二次元配列に格納
        int[,] textWords = new int[columnNum, rowNum];
        for(int i = 0; i < columnNum; i++){
            string[] tempWords = textData[i].Split(',');
            for(int j = 0; j < rowNum; j++){
                int type = int.Parse(tempWords[j]);
                textWords[i,j] = type;
                // フルーツの種類格納
                int num = stageNum - 1;
                if(this.fruitsTypeList.Count <= num){
                    this.fruitsTypeList.Add(new List<int>(){type});
                }else if(!this.fruitsTypeList[num].Contains(type)){
                    this.fruitsTypeList[num].Add(type);
                }
            }
        }
        return textWords;
    }
}
