using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace PlayScene
{
    public class GameManager : MonoBehaviour
    {
        public FruitsMap fruitsMapScript;
        public PlayScene.UIManager uiManagerScript;
        private AudioManager audioManager;
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
            this.audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        // 更新処理
        private void Update(){
            if(!this.isPlayEndFlg){
                if(this.isClearFlg){
                    this.isPlayEndFlg = true;
                    this.fruitsMapScript.playAllMatchedBlink();
                    this.audioManager.playSE(SE_TYPE.FRUITS_ALL_MATCH);
                    Invoke("showGameClearUI", 1.5f);
                }else{
                    this.timeNum += Time.deltaTime;
                    this.uiManagerScript.timeText.text = "タイム：" + this.timeNum.ToString("f0").PadLeft(4);
                }
            }
        }

        private void saveData(){
            StageInfo stageInfo = new StageInfo();
            // 記録するKey作成
            string key = "stage" + this.stageNum.ToString();
            // 前回の情報取得
            StageInfo beforeStageInfo = PlayerPrefsUtils.GetObject<StageInfo>(key);
            // 前回の情報がある場合
            if(beforeStageInfo != null){
                // 時間
                stageInfo.timeNum = (this.timeNum < beforeStageInfo.timeNum) ? this.timeNum : beforeStageInfo.timeNum;
                // タップ数
                stageInfo.tapNum = (this.tapNum < beforeStageInfo.tapNum) ? this.tapNum : beforeStageInfo.tapNum;
                // 指定時間以内のクリア判定
                stageInfo.clearList[0] = (!beforeStageInfo.clearList[0]) ? this.timeNum < 20.0f : beforeStageInfo.clearList[0];
                // 指定タップ数以内のクリア判定
                stageInfo.clearList[1] = (!beforeStageInfo.clearList[1]) ? this.tapNum < 20 : beforeStageInfo.clearList[1];
                // 時間とタップ数が両方指定数以内のクリア判定
                stageInfo.clearList[2] = (!beforeStageInfo.clearList[2]) ? this.timeNum < 20.0f && this.tapNum < 20 : beforeStageInfo.clearList[2];
            // 新規情報追加の場合
            }else{
                // 時間
                stageInfo.timeNum = this.timeNum;
                // タップ数
                stageInfo.tapNum = this.tapNum;
                // 指定時間以内のクリア判定
                stageInfo.clearList[0] = this.timeNum < 5.0f;
                // 指定タップ数以内のクリア判定
                stageInfo.clearList[1] = this.tapNum < 5;
                // 時間とタップ数が両方指定数以内のクリア判定
                stageInfo.clearList[2] = this.timeNum < 5.0f && this.tapNum < 5;
            }
            // データを保存する
            PlayerPrefsUtils.SetObject(key, stageInfo);
        }

        // ゲームクリアUI表示
        private void showGameClearUI(){
            this.uiManagerScript.showGameClearUI();
            this.audioManager.playSE(SE_TYPE.GAME_CLEAR);
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
