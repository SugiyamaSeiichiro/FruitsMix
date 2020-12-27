using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int timeNum;
    public int tapNum;
    public int stageNum;
    public int squareNum;
    public int rowNum;
    public int columnNum;
    public int fruitsTypeNum;
    public List<int> fruitsTypeList;
    public bool verticalFlg;
    public bool horizontalFlg;
    public bool diagonalFlg;
    public bool isClearFlg;
    public int[,] compMap;

    // Use this for initialization
    void Start () {
        this.timeNum = 0;
        this.tapNum = 0;
        this.isClearFlg = false;
    }

    void Update(){
        if(this.isClearFlg){
            Debug.Log("クリア");
        }else{
            this.timeNum = (int)Time.time;
        }
    }

    //private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    //{
        // シーン切り替え後のスクリプトを取得
        //var gameManager= GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        
        // データを渡す処理
        //gameManager.score = 100;

        // イベントから削除
        //SceneManager.sceneLoaded -= GameSceneLoaded;
    //}
}
