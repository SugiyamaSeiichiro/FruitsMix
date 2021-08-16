using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

namespace SelectScene
{
    public class StageInfoModal : MonoBehaviour
    {
        private GameCommon gameCommonScript;
        public GameObject stageInfoModal;
        public Text titleText;
        public Text squareNumText;
        public Text typeNumText;
        public Text bestTimeNumText;
        public Text bestTapNumText;
        public GameObject fruitsImagePrefab;
        public GameObject fruitsInitMapParent;
        public GameObject fruitsCompMapParent;
        public GameObject starImagesParent;
        public GameObject directionConditionTextParent;
        private int rowNum;
        private int columnNum;
        private int stageNum;
        private int squareNum;
        private int typeNum;
        private Dictionary<string, bool> directionConditionList;
        private StageInfo stageInfo;
        private readonly Dictionary<int, float> fruitsSizeList = new Dictionary<int, float>(){
            {3, 42.0f},
            {4, 30.0f},
            {5, 25.0f},
        };
        private AudioManager audioManager;

        void Start(){
            this.audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        // ステージボタン情報設定
        public void setStageButtonInfo(int stageNum){
            // 初期化
            this.gameCommonScript = GameObject.Find("GameCommon").GetComponent<GameCommon>();
            this.stageNum = stageNum;
            this.rowNum = this.gameCommonScript.getRowNum(this.stageNum);
            this.columnNum = this.gameCommonScript.getColumnNum(this.stageNum);
            this.squareNum = this.gameCommonScript.getSquareNum(this.stageNum);
            this.typeNum = this.gameCommonScript.getFruitsTypeNum(this.stageNum);
            this.directionConditionList = this.gameCommonScript.getDirectionConditionList(this.stageNum);
            // クリア情報取得
            string key = "stage" + this.stageNum.ToString();
            this.stageInfo = PlayerPrefsUtils.GetObject<StageInfo>(key);
            // 文字の作成
            this.setText();
            // ステージボタン設定
            this.setStageButton();
            // 星UI作成
            this.setStarUI();
            // フルーツUI作成
            int[,] initMap = this.gameCommonScript.getFruitsInitMap(this.stageNum);
            int[,] compMap = this.gameCommonScript.getFruitsCompMap(this.stageNum);
            this.setFruitsMapUI(initMap, this.fruitsInitMapParent);
            this.setFruitsMapUI(compMap, this.fruitsCompMapParent);
        }

        // テキスト作成
        private void setText(){
            this.titleText.text = "Stage " + this.stageNum.ToString();
            this.typeNumText.text = this.typeNum.ToString() + "種類";
            this.squareNumText.text = this.rowNum.ToString() + "×" + this.columnNum.ToString() + "マス";
            // 方向条件
            string[] directionKeyList = {"vertical", "horizontal", "diagonal"};
            for(int i = 0; i < this.directionConditionTextParent.transform.childCount; i++){
                if(!this.directionConditionList[directionKeyList[i]]){
                    this.directionConditionTextParent.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            // 最高記録
            if(this.stageInfo != null && this.stageInfo.clearFlg){
                this.bestTimeNumText.text = "BestTime: " + this.stageInfo.timeNum.ToString("f0") + "sec";
                this.bestTapNumText.text = "BestTap: " + this.stageInfo.tapNum.ToString();
            }
        }

        // ステージボタン設定
        private void setStageButton(){
            // クリア済の場合、外郭の色を変更
            if(this.stageInfo != null && this.stageInfo.clearFlg){
                this.stageInfoModal.GetComponent<Outline>().effectColor = Color.yellow;
            }
            // 難易度別に色を変更
            // マス数と種類数の合計
            int level = this.squareNum - 3;
            this.stageInfoModal.GetComponent<Image>().color = Utility.getLevelColor(level);
        }

        // 星UI作成
        private void setStarUI(){
            List<string> missionTextList = Utility.getMissionTextList(this.typeNum, this.squareNum);

            for(int i = 0; i < this.starImagesParent.transform.childCount; i++){
                GameObject obj = this.starImagesParent.transform.GetChild(i).gameObject;
                if(this.stageInfo == null || !this.stageInfo.clearList[i]){
                    obj.GetComponent<Image>().color = Color.gray;
                }
                Text text = obj.transform.GetChild(0).gameObject.GetComponent<Text>();
                text.text = missionTextList[i];
            }
        }

        // フルーツUI作成
        private void setFruitsMapUI(int[,] map, GameObject parent){
            // フルーツ配置箇所取得
            float fruitsSize = this.fruitsSizeList[this.squareNum];
            List<float> fruitsPosList = this.gameCommonScript.getFruitsPosList(this.squareNum, fruitsSize);
            for(int i = 0; i < this.columnNum; i++){
                for(int j = 0; j < this.rowNum; j++){
                    GameObject obj = Instantiate(this.fruitsImagePrefab);
                    obj.GetComponent<Image>().sprite = this.gameCommonScript.getFrutisSprite(map[i,j]);
                    obj.transform.SetParent(parent.transform, false);
                    RectTransform rectTransform = obj.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(fruitsSize, fruitsSize);
                    rectTransform.anchoredPosition = new Vector2(fruitsPosList[j], -fruitsPosList[i]);
                }
            }
        }

        // モーダル削除
        public void onClickToDestroy(){
            Destroy(this.gameObject);
            this.audioManager.playSE(SE_TYPE.BUTTON);
        }

        // プレイシーン移動
        public void onClickToPlayScene(){
            SceneManager.sceneLoaded += GameSceneLoaded;
            SceneManager.LoadScene(Define.PlayScene);
            this.audioManager.playSE(SE_TYPE.BUTTON);
        }

        private void GameSceneLoaded(Scene next, LoadSceneMode mode)
        {
            //シーン切り替え後のスクリプトを取得
            var gameManager = GameObject.FindWithTag("GameManager").GetComponent<PlayScene.GameManager>();
            //データを渡す処理
            gameManager.stageNum = this.stageNum;
            //イベントから削除
            SceneManager.sceneLoaded -= GameSceneLoaded;
        }
    }
}
