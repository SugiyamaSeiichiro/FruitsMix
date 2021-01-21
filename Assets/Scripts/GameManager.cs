using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayScene
{
    public class GameManager : MonoBehaviour
    {
        public FruitsMap fruitsMapScript;
        public PlayScene.UIManager uiManagerScript;
        public AudioManager audioManagerScript;
        public float timeNum;
        public int tapNum;
        public int stageNum;
        public int squareNum;
        public int rowNum;
        public int columnNum;
        public int fruitsTypeNum;
        public float fruitsIntervalIndex;
        public List<int> fruitsTypeList;
        public List<Sprite> fruitsSpriteList;
        public int[,] initMap;
        public int[,] compMap;
        public bool verticalFlg;
        public bool horizontalFlg;
        public bool diagonalFlg;
        public bool isClearFlg;

        private bool isPlayEndFlg = false;

        // 初期処理
        private void Start () {
            // 初期化
            this.timeNum = 0.0f;
            this.tapNum = 0;
            this.isClearFlg = false;
            // ステージの各パラメーター設定
            this.setStageParameter(this.stageNum);
        }

        // 更新処理
        private void Update(){
            if(!this.isPlayEndFlg){
                if(this.isClearFlg){
                    this.isPlayEndFlg = true;
                    this.fruitsMapScript.playAllMatchedBlink();
                    this.audioManagerScript.playGameClearSE();
                    Invoke("showGameClearUI", 1.5f);
                }else{
                    this.timeNum += Time.deltaTime;
                    this.uiManagerScript.timeText.text = "タイム：" + this.timeNum.ToString("f0").PadLeft(4);
                    this.uiManagerScript.tapText.text = "タップ：" + this.tapNum.ToString().PadLeft(4);
                }
            }
        }

        // ゲームクリアUI表示
        private void showGameClearUI(){
            this.uiManagerScript.showGameClearUI();
        }

        // ステージの各パラメーター設定
        private void setStageParameter(int stageNum){
            // 共通Script取得
            GameCommon gameCommonScript = GameObject.Find("GameCommon").GetComponent<GameCommon>();
            // 行数取得
            this.rowNum = gameCommonScript.getRowNum(stageNum);
            // 烈数取得
            this.columnNum = gameCommonScript.getColumnNum(stageNum);
            // マス数取得
            this.squareNum = gameCommonScript.getSquareNum(stageNum);
            // 初期配置取得
            this.initMap = gameCommonScript.getFruitsInitMap(stageNum);
            // 完成配置所得
            this.compMap = gameCommonScript.getFruitsCompMap(stageNum);
            // 種類順番取得
            this.fruitsTypeList = gameCommonScript.getFrutisTypeList(stageNum);
            // 種類数取得
            this.fruitsTypeNum = gameCommonScript.getFruitsTypeNum(stageNum);
            // フルーツSprite取得
            this.fruitsSpriteList = gameCommonScript.fruitsSpriteList;
            // フルーツ同士の間隔係数
            this.fruitsIntervalIndex = gameCommonScript.fruitsIntervalIndex;
            // 方向条件取得
            Dictionary<string, bool> directionConditionList = gameCommonScript.getDirectionConditionList(stageNum);
            // 縦方向条件取得
            this.verticalFlg = directionConditionList["vertical"];
            // 横方向条件取得
            this.horizontalFlg = directionConditionList["horizontal"];
            // 斜め方向条件取得
            this.diagonalFlg = directionConditionList["diagonal"];
        }
    }
}
